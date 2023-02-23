using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto.Subscription
{
    public class Subscription_QL
    {
        public long SubscriptionId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int CheckMinutes { get; set; }
    }
}
