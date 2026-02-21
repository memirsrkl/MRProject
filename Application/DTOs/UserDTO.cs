using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserDTO
    {
    }
    public class UserLoginDto 
    {
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
