using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto
{
    public record UserQL
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
    public record UserQLInput(long UserId, string Username, byte[] Password);
    public record UserQLPayload(long UserId, string Username);
}
