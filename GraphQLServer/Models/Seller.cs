using System;
using System.Collections.Generic;

namespace GraphQLServer.Models
{
    public partial class Seller
    {
        public Seller()
        {
            Products = new HashSet<Product>();
        }

        public long SellerId { get; set; }
        public string SellerName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
