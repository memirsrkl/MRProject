
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums;

namespace Domain.Entities
{
    public class ReservationUser:BaseEntitiy
    {
        public Guid UserId { get; set; }
        public Guid MeetingRoomId { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public Users User { get; set; }
        public ParticipantType Type { get; set; }
        public MeetingRooms MeetingRooms { get; set; }
    }
}
