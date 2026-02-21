using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MeetingRoom.Querie
{
    public class GetCreateMeetingRoomQuery:IRequest<UserCreateMeetingDTO>
    {
        public string currentUserName { get; set; }
        public GetCreateMeetingRoomQuery(string currentUserName)
        {
            this.currentUserName = currentUserName;
        }
        public class GetCreateMeetingRoomQueryHandler : IRequestHandler<GetCreateMeetingRoomQuery, UserCreateMeetingDTO>
        {
            private readonly IApplicationDbContext _context;
            public GetCreateMeetingRoomQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<UserCreateMeetingDTO> Handle(GetCreateMeetingRoomQuery request, CancellationToken cancellationToken)
            {
                UserCreateMeetingDTO meetingDto = new UserCreateMeetingDTO();
                var user = _context.Users.Where(t => t.UserName == request.currentUserName).FirstOrDefault();
                var meeting = _context.ReservationUsers.Include(t => t.Reservation).Include(t => t.User).Select(t => new UserReservationList
                {
                    Date = t.Reservation.ReservationDate,
                    EndDate = t.Reservation.EndTime,
                    StartDate = t.Reservation.StartTime,
                    Id = t.Id
                }).ToList();
                var meetingRoom = _context.MeetingRooms.Include(t=>t.ReservationUsers).Select(t=>new MeetingRoomDTO
                {
                    Id=t.Id,
                    Location=t.Location,
                    Name=t.Description,
                    TotalRequest = t.ReservationUsers
            .Count(r => r.Reservation.ReservationDate >= DateTime.Now &&
                        r.Reservation.ReservationDate <= DateTime.Now.AddDays(14))
                });
                meetingDto.MeetingsRoom = meetingRoom.ToList();
                meetingDto.UserReservationList = meeting.ToList();
                return meetingDto;
            }
        }
    }
}
