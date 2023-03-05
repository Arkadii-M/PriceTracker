using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaDto
{
    public class RozetkaPageResult
    {
        public string product_title { get; set; } = string.Empty;
        public decimal price { get; set; }
        public DateTime datetime { get; set; }
        public bool in_stock { get; set; }
        public string seller_name { get; set; }
     }
}
