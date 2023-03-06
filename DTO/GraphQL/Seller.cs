using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GraphQL
{
    public record SellerQL
    {
        public long SellerId { get; set; }
        public string SellerName { get; set; }

        public ICollection<ProductQL> Products { get; set; }
    }
    public record SellerQLInput(string SellerName);
    public record SellerQLPayload : SellerQL;
}
