using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
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
    public class RemoveParticipantCommand:IRequest<Response<bool>>
    {
        public MettingAddParticipant model { get; set; }
        public string currentUserName { get; set; }
        public RemoveParticipantCommand(MettingAddParticipant model, string currentUserName)
        {
            this.model = model;
            this.currentUserName = currentUserName;
        }
        public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public RemoveParticipantCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<bool>> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
            {

                var reservationUser = _context.ReservationUsers.Include(t=>t.Reservation).Where(t => t.Id == request.model.UserId).FirstOrDefault();
                if(reservationUser==null || reservationUser.Reservation == null)
                {
                    return new Response<bool>("Rezervasyon bulunamadı");
                }
                if (reservationUser.Reservation.Status != Domain.Enums.ReservationStatus.Approved)
                    return new Response<bool>("Sadece onaylanmış rezervasyon düzenlenebilir");
                if (reservationUser.Reservation.ReservationDate.Date <= DateTime.Today)
                    return new Response<bool>("Geçmiş rezervasyon düzenlenemez");
                if (reservationUser.Type == Domain.Enums.ParticipantType.Creator)
                    return new Response<bool>("Toplantı sahibi silinemez");
                _context.ReservationUsers.Remove(reservationUser);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>(true);
            }
        }
    }
}
