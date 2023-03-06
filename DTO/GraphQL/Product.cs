using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GraphQL
{
    public record ProductQL
    {
        public long ProductId { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public long SellerId { get; set; }
    }
    public record ProductQLInput(string Link, string Name, long SellerId);
    public record ProductQLPayload: ProductQL;

}
