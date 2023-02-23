using System;
using System.Collections.Generic;

namespace GraphQLServer.Models
{
    public partial class Product
    {
        public Product()
        {
            Histories = new HashSet<History>();
            Subscriptions = new HashSet<Subscription>();
        }

        public long ProductId { get; set; }
        public string Link { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long SellerId { get; set; }

        public virtual Seller Seller { get; set; } = null!;
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
