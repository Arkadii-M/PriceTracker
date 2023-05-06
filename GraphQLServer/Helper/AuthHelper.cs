using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GraphQLServer.Helper
{
    public class AuthHelper
    {
        public static string Issuer
        {
            get
            {
                return Environment.GetEnvironmentVariable("JWT_AUTH_ISSUER") ?? throw new ArgumentException("Missing env var: AUTH_ISSUER");
            }
        }

        public static string Audience
        {
            get
            {
                return Environment.GetEnvironmentVariable("JWT_AUTH_AUDIENCE") ?? throw new ArgumentException("Missing env var: AUTH_AUDIENCE");
            }
        }
        public static readonly bool VailidateLifetime = false;
        public static readonly int TokenLifetime = 10;
        private static string SigningKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("JWT_AUTH_KEY") ?? throw new ArgumentException("Missing env var: AUTH_AUDIENCE");
            }
        }
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        }
    }
}
