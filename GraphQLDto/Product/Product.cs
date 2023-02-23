using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto.Product
{
    public class Product_QL
    {
        public long ProductId { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public long SellerId { get; set; }
    }
}
