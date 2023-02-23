using System;
using System.Collections.Generic;

namespace GraphQLServer.DbModels
{
    public partial class User
    {
        public User()
        {
            Subscriptions = new HashSet<Subscription>();
        }

        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public string Salt { get; set; } = null!;

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
