using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto.User
{
    public record UserPayload_QL(long UserId, string Username);
}
