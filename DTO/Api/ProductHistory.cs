using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Api
{
    public class ProductPriceHistory
    {
        public Product Product { get; set; }
        public ICollection<ProductHistory> ProductHistory { get; set; } = null;
    }
    public record ProductHistory(long HistoryId, long ProductId, DateTime Datetime, decimal Price, bool InStock);
}
