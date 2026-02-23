using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class MeetingsDTO
    {
        public string Date { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RoomName { get; set; }
        public List<UserListForMettings> MeetingPersson { get; set;  }
        public Guid Id { get; set; }
    }
    public class MeetingsCreatorDTO
    {
        public DateTime Date { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RoomName { get; set; }
        public string Status { get; set; }
        public List<UserListForMettings> MeetingPersson { get; set; }
        public Guid Id { get; set; }
    }
}
