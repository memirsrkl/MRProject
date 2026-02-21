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
    public class AddUserDto
    {
        public Guid Id { get; set; }
        public string UserFullName { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class UserListDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
    }
}
