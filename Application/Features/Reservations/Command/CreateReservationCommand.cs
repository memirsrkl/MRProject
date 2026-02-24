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
                if (request.dtos.Date.Date < DateTime.Today)
                    return new Response<bool>("Geçmiş tarihli rezervasyon oluşturulamaz");
                var today = DateTime.Today;
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                var startOfThisWeek = today.AddDays(-diff);
                var endOfNextWeek = startOfThisWeek.AddDays(13);

                if (request.dtos.Date.Date < startOfThisWeek ||
                    request.dtos.Date.Date > endOfNextWeek)
                {
                    return new Response<bool>(
                        "Sadece bu hafta ve gelecek hafta için rezervasyon oluşturabilirsiniz");
                }
                var isConflict = _context.Reservations.Any(r =>
    r.MeetingRoomId == request.dtos.RoomId &&
    r.ReservationDate.Date == request.dtos.Date.Date &&
    r.Status != Enums.ReservationStatus.Rejected &&
    request.dtos.Start < r.EndTime &&
    request.dtos.End > r.StartTime
);

                if (isConflict)
                    return new Response<bool>(
                        "Bu tarih ve saat aralığında oda doludur");
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
