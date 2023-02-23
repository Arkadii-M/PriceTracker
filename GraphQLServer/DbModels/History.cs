using System;
using System.Collections.Generic;

namespace GraphQLServer.DbModels
{
    public partial class History
    {
        public long HistoryId { get; set; }
        public long ProductId { get; set; }
        public DateTime Datetime { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
