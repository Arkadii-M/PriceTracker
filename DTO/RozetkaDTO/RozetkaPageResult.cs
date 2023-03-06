using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Rozetka
{
    public class RozetkaPageResult
    {
        public long? id { get; set; } = null;
        public string url { get; set; } = string.Empty;
        public string product_title { get; set; } = string.Empty;
        public decimal price { get; set; }
        public DateTime datetime { get; set; }
        public bool in_stock { get; set; }
        public string seller_name { get; set; } = string.Empty;
     }
}
