using GraphQLDto.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto.Update
{
    public class Update_QL
    {
        public long SubscriptionId { get; set; }
        public long? HistoryId { get; set; }
        public DateTime ToCheck { get; set; }

        public virtual History_QL? History { get; set; }
        public virtual Subscription.Subscription_QL Subscription { get; set; }
    }
}
