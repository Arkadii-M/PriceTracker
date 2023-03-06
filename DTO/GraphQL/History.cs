using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GraphQL
{
    public record HistoryQL
    {
        //[DefaultValue(-1)]
        //public Optional<long> HistoryId { get; set; }
        public long HistoryId { get; set; }
        public long ProductId { get; set; }
        public DateTime Datetime { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public ProductQL Product { get; set; } = null!;
    }
    public record HistoryQLInput(long ProductId, DateTime Datetime, decimal Price, bool InStock);
    public record HistoryQLPayload : HistoryQL;
}
