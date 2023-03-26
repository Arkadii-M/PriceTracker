using DTO.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Api
{
    public class Subscription
    {
        public long SubscriptionId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int CheckMinutes { get; set; }


        public Product? Product { get; set; }
    }
}
