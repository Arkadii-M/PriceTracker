using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto
{
    public record UserPayloadQL(long UserId, string Username);
}
