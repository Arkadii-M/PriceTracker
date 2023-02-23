using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto.History
{
    public class History_QL
    {
        public long HistoryId { get; set; }
        public long ProductId { get; set; }
        public DateTime Datetime { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

    }
}
