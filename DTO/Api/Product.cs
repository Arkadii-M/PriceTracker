using DTO.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Api
{
    //public class Product
    //{
    //    public long ProductId { get; set; }
    //    public string Link { get; set; }
    //    public string Name { get; set; }
    //    public long SellerId { get; set; }
    //    public decimal LastPrice { get; set; }
    //}


    public class Product
    {
        public long ProductId { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public Seller Seller { get; set; }

        public decimal LastPrice { get; set; }
        public bool InStock { get; set; }

        public ICollection<ProductHistory>? History { get; set; } = null;
    }
    public record ProductHistory(long HistoryId, long ProductId, DateTime Datetime, decimal Price, bool InStock);
}
