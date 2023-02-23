using System;
using System.Collections.Generic;

namespace GraphQLServer.DbModels
{
    public partial class Update
    {
        public long SubscriptionId { get; set; }
        public long? HistoryId { get; set; }
        public DateTime ToCheck { get; set; }

        public virtual History? History { get; set; }
        public virtual Subscription Subscription { get; set; } = null!;
    }
}
