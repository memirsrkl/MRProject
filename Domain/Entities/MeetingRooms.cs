using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MeetingRooms:BaseEntitiy
    {
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public ICollection<ReservationUser> ReservationUsers { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

    }
}
