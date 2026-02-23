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

namespace Application.Features.Reservations.Query
{
    public class GetAllReservationQuery:IRequest<List<AdminReservationDTO>>
    {
        public string currentUserName { get; set; }
        public GetAllReservationQuery(string currentUserName)
        {
            this.currentUserName = currentUserName;
        }
        public class GetAllReservationQueryHandler : IRequestHandler<GetAllReservationQuery, List<AdminReservationDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllReservationQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<AdminReservationDTO>> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
            {
                var userIsAdmin = _context.Users.FirstOrDefault(t => t.UserName == request.currentUserName).IsAdmin;
                if (userIsAdmin)
                {
                    var meetingList = _context.Reservations.Include(t => t.ReservationUsers).ThenInclude(y => y.User).Include(t => t.MeetingRoom).Select(k => new AdminReservationDTO
                    {
                        Date = k.ReservationDate.ToShortDateString(),
                        Id = k.Id,
                        End = k.EndTime.ToString("HH:mm"),
                        Start = k.StartTime.ToString("HH:mm"),
                        MeetingRoomName = k.MeetingRoom.Description,

                        UserList = k.ReservationUsers.Select(t => new UserListForMettings
                        {
                            IsCreator = t.Type == Enums.ParticipantType.Participant ? false : true,
                            FullName = t.User.GetFullName()
                        }).ToList(),
                        BackgroundColor= EnumExtensions.GetColor(k.Status),
                        BorderColor= EnumExtensions.GetColor(k.Status),
                        TextColor="black",
                        StatusName=EnumExtensions.GetName(k.Status),
                    }).ToList();
                    return meetingList;
                }
                else
                {
                    return null;
                }
                
            }
        }
    }
}
