using Domain.Common;
using Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Users: BaseEntitiy
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Encrypted]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<ReservationUser> ReservationUser { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }

    }
}
