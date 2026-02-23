using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Meetings.Queries
{
    public class GetUserCreaterPersonMeetingListQuery : IRequest<List<MeetingsCreatorDTO>>
    {
        public string name { get; set; }
        public GetUserCreaterPersonMeetingListQuery(string name)
        {
            this.name = name;
        }
        public class GetUserCreaterPersonMeetingListQueryHandler : IRequestHandler<GetUserCreaterPersonMeetingListQuery, List<MeetingsCreatorDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserCreaterPersonMeetingListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<MeetingsCreatorDTO>> Handle(GetUserCreaterPersonMeetingListQuery request, CancellationToken cancellationToken)
            {
                var id = _context.Users.FirstOrDefault(t => t.UserName == request.name).Id;
                var meetingList = _context.Reservations.Include(t => t.ReservationUsers).ThenInclude(y => y.User).Include(t => t.MeetingRoom).Where(t => t.ReservationUsers.Any(t => t.UserId == id && t.Type == Enums.ParticipantType.Creator)).Select(k => new MeetingsCreatorDTO
                {
                    Date = k.ReservationDate,
                    Id = k.Id,
                    EndDate = k.EndTime.ToString("HH:mm"),
                    StartDate = k.StartTime.ToString("HH:mm"),
                    RoomName = k.MeetingRoom.Description,
                    Status=EnumExtensions.GetName(k.Status),
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
