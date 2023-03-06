using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Api
{
    public class User
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = string.Empty;
    }
}
