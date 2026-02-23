using Application.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Meetings.Queries
{
    public class GetUserPersonMeetingListQuery:IRequest<List<MeetingsDTO>>
    {
        public string name { get; set; }
        public GetUserPersonMeetingListQuery(string name)
        {
            this.name = name;
        }
        public class GetUserPersonMeetingListQueryHandler : IRequestHandler<GetUserPersonMeetingListQuery, List<MeetingsDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserPersonMeetingListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<MeetingsDTO>> Handle(GetUserPersonMeetingListQuery request, CancellationToken cancellationToken)
            {
                var id = _context.Users.FirstOrDefault(t => t.UserName == request.name).Id;
                var meetingList = _context.Reservations.Include(t => t.ReservationUsers).ThenInclude(y => y.User).Include(t => t.MeetingRoom).Where(t=>t.ReservationUsers.Any(t=>t.UserId==id)).Select(k => new MeetingsDTO
                {
                    Date = k.ReservationDate.ToShortDateString(),
                    Id = k.Id,
                    EndDate = k.EndTime.ToString("HH:mm"),
                    StartDate = k.StartTime.ToString("HH:mm"),
                    RoomName = k.MeetingRoom.Description,
                    Subject=k.MeetingSubject,
                    MeetingPersson = k.ReservationUsers.Select(t => new UserListForMettings
                    {
                        IsCreator = t.Type == Enums.ParticipantType.Participant ? false : true,
                        FullName = t.User.GetFullName()
                    }).ToList(),
                }).ToList();
                return meetingList;
            }
        }
    }
}
