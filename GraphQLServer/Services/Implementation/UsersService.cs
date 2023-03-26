using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GraphQLServer.Services
{
    public class UserService : IUserService, IAsyncDisposable
    {
        private readonly IMapper _mapper;
        private readonly PriceTrackerContext _dbContext;

        public UserService(IDbContextFactory<PriceTrackerContext> dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
        }

        public UserQLPayload CreateUser(CreateUserQLInput user)
        {
            Guid salt = Guid.NewGuid();
            var user_db = new User
            {
                Username = user.Username,
                Password = hash(user.Password, salt.ToString()),
                Salt = salt.ToString()
            };
            _dbContext.Users.Add(user_db);
            _dbContext.SaveChanges();
            return _mapper.Map<UserQLPayload>(user_db);
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public IQueryable<UserQLPayload> GetAll()
        {
            var users = _dbContext.Users.Include(i => i.Subscriptions).ThenInclude(p => p.Product);
            List<UserQLPayload> res = new List<UserQLPayload>();
            foreach (var user in users)
                res.Add(_mapper.Map<UserQLPayload>(user));

            return res.AsQueryable();
            //return _mapper.ProjectTo<UserQLPayload>(_dbContext.Users.Include(i => i.Subscriptions).ThenInclude(p => p.Product).AsQueryable());
        }

        public UserQLPayload GetUserById(long id)
        {
            return _mapper.Map<UserQLPayload>(_dbContext.Users
                .Include(s => s.Subscriptions)
                .FirstOrDefault(cond => cond.UserId == id));
        }

        public LoginUserQLPayload LoginUser(LoginUserQLInput user_data)
        {

            var user = _dbContext.Users.FirstOrDefault(u => u.Username == user_data.Username);
            if (user is null)
                return new LoginUserQLPayload(user_data.Username, false);

            if(user.Password.SequenceEqual(hash(user_data.Password, user.Salt)))
                return new LoginUserQLPayload(user_data.Username, true);


            return new LoginUserQLPayload(user_data.Username, false);
        }


        private byte[] hash(string password, string salt)
        {
            var alg = SHA512.Create();
            return alg.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        }
    }
}
