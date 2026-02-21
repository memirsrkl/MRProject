using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserCreateMeetingDTO
    {
        public List<MeetingRoomDTO> MeetingsRoom { get; set; }
        public List<UserReservationList> UserReservationList { get; set; }
    }
    public class MeetingRoomDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int TotalRequest { get; set; }
    }
    public class UserReservationList
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Date { get; set; }
    }
}
