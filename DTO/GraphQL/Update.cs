using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GraphQL
{
    public record UpdateQL
    {
        public long SubscriptionId { get; set; }
        public long? HistoryId { get; set; }
        public DateTime ToCheck { get; set; }

        public HistoryQL? History { get; set; }
        public SubscriptionQL? Subscription { get; set; }
    }
    public record UpdateQLInput(long SubscriptionId, long HistoryId, DateTime ToCheck);
    public record UpdateQLPayload : UpdateQL;
}
