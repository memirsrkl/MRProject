using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums;

namespace Domain.Entities
{
    public class Reservation:BaseEntitiy
    {
        public Guid UserId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid MeetingRoomId { get; set; }
        public ReservationStatus Status { get; set; }
        public MeetingRooms MeetingRoom { get; set; }
        public Users User { get; set; }
        public ICollection<ReservationUser> ReservationUsers { get; set; }
    }
}
