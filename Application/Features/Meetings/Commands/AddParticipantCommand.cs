using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Meetings.Commands
{
    public class AddParticipantCommand:IRequest<Response<bool>>
    {
        public MettingAddParticipant model { get; set; }
        public AddParticipantCommand(MettingAddParticipant model)
        {
            this.model = model;
        }
        public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public AddParticipantCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<bool>> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
            {
                var reservation = _context.Reservations.FirstOrDefault(t => t.Id == request.model.MeetingId);
                if (reservation != null)
                {
                    if (reservation.Status != Enums.ReservationStatus.Approved)
                        return new Response<bool>(
                            "Sadece onaylanmış rezervasyon düzenlenebilir");
                    if (reservation.ReservationDate.Date <= DateTime.Today)
                        return new Response<bool>(
                            "Geçmiş rezervasyon düzenlenemez");
                    if (reservation.ReservationDate.Date <= DateTime.Today)
                        return new Response<bool>(
                            "Geçmiş rezervasyon düzenlenemez");
                    var alreadyExists = _context.ReservationUsers.Any(x =>x.ReservationId == request.model.MeetingId &&      x.UserId == request.model.UserId);
                    if (alreadyExists)
                        return new Response<bool>("Bu kullanıcı zaten ekli");
                    var hasConflict = _context.ReservationUsers.Include(x => x.Reservation).Any(x =>x.UserId == request.model.UserId &&
           x.Reservation.ReservationDate.Date ==
           reservation.ReservationDate.Date &&
           request.model.MeetingId != x.ReservationId &&
           reservation.StartTime < x.Reservation.EndTime &&
           reservation.EndTime > x.Reservation.StartTime &&
           x.Reservation.Status != Enums.ReservationStatus.Rejected);

                    if (hasConflict)
                        return new Response<bool>(
                            "Kullanıcı bu saat aralığında başka toplantıda");
                    ReservationUser user = new ReservationUser();
                    user.ReservationId = request.model.MeetingId;
                    user.UserId = request.model.UserId;
                    user.CreatedTime = DateTime.Now;
                    user.Type = Enums.ParticipantType.Participant;
                    user.MeetingRoomId = reservation.MeetingRoomId;
                    _context.ReservationUsers.Add(user);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new Response<bool>(true);
                }
                else
                {
                    return new Response<bool>("İlgili rezervasyon bulunamadı");
                }
                
            }
        }
    }
}
