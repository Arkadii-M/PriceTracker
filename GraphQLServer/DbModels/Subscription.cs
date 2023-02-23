using System;
using System.Collections.Generic;

namespace GraphQLServer.DbModels
{
    public partial class Subscription
    {
        public long SubscriptionId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int CheckMinutes { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
