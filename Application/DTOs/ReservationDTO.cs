using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReservationDTO
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public string TextColor { get; set; }
    }
    public  class CreateReservationDto
    {
        public Guid RoomId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<Guid> ParticipantIds { get; set; }
    }
    public class MettingAddParticipant
    {
        public Guid MeetingId { get; set; }
        public Guid UserId { get; set; }

    }
    public class AdminReservationDTO
    {
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public List<UserListForMettings> UserList { get; set; }
        public string MeetingRoomName { get; set; }
        public string StatusName { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public string TextColor { get; set; }
        public Guid Id { get; set; }
    }
}
