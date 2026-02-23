using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using Domain;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservations.Command
{
    public class CreateReservationCommand : IRequest<Response<bool>>
    {
        public CreateReservationDto dtos { get; set; }
        public string CurrentUserName { get; set; }
        public CreateReservationCommand(CreateReservationDto dtos, string currentUserName)
        {
            this.dtos = dtos;
            CurrentUserName = currentUserName;
        }
        public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public CreateReservationCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<bool>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
            {
                var user = _context.Users.FirstOrDefault(t => t.UserName == request.CurrentUserName);
                Reservation reserv = new Reservation
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTime.Now,
                    EndTime = request.dtos.End,
                    IsDeleted = false,
                    MeetingSubject =request.dtos.Subject,
                    MeetingRoomId = request.dtos.RoomId,
                    ReservationDate = request.dtos.Date,
                    StartTime = request.dtos.Start,
                    Status = Enums.ReservationStatus.Pending,
                    UserId = user.Id
                };
                _context.Reservations.Add(reserv);
                await _context.SaveChangesAsync(cancellationToken);
                List<ReservationUser> rvUser = new List<ReservationUser>();
                foreach (var us in request.dtos.ParticipantIds)
                {
                    var usId = Guid.Parse(us.ToString());
                    var userew = _context.Users.Where(t => t.Id == usId).FirstOrDefault();
                    var reservationExists = _context.Reservations.Any(x => x.Id == reserv.Id);
                    ReservationUser resUser = new ReservationUser
                    {
                        Id=Guid.NewGuid(),
                        ReservationId=reserv.Id,
                        CreatedTime = DateTime.Now,
                        MeetingRoomId = reserv.MeetingRoomId,
                        UserId = usId,
                    };
                    resUser.Type = user.Id == usId ? Enums.ParticipantType.Creator : Enums.ParticipantType.Participant;
                    rvUser.Add(resUser);
                    _context.ReservationUsers.Add(resUser);
                    
                }
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {

                }
                return new Response<bool>(true);
            }
        }
    }
}
