using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Api
{
    public class ProductPriceHistory
    {
        Product Product { get; set; }
        ICollection<ProductHistory> ProductHistory { get; set; } = null;
    }
    public record ProductHistory(long HistoryId, long ProductId, DateTime Datetime, decimal Price, bool InStock);
}
