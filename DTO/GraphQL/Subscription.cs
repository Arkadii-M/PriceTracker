using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GraphQL
{
    public record SubscriptionQL
    {
        public long SubscriptionId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int CheckMinutes { get; set; }

        public virtual ProductQL Product { get; set; }
        public virtual UserQL User { get; set; }
    }
    public record SubscriptionQLUpdate(long SubscriptionId,long UserId, long ProductId, int CheckMinutes);
    
    public record SubscriptionQLInput(long UserId, long ProductId, int CheckMinutes);
    public record SubscriptionQLPayload : SubscriptionQL;
}
