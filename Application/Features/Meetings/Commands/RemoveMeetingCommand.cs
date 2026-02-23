using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums;

namespace Application.Features.Meetings.Commands
{
    public class RemoveMeetingCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public string currentUserName { get; set; }
        public RemoveMeetingCommand(Guid id, string currentUserName )
        {
            Id = id;
            this.currentUserName = currentUserName;
        }
        public class RemoveMeetingCommandHandler : IRequestHandler<RemoveMeetingCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public RemoveMeetingCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<bool>> Handle(RemoveMeetingCommand request, CancellationToken cancellationToken)
            {
                var reservation = await _context.Reservations
            .Include(x => x.ReservationUsers).Include(t=>t.User)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (reservation == null)
                    return new Response<bool>("Rezervasyon bulunamadı");

                var creator = reservation.ReservationUsers
                    .FirstOrDefault(x => x.Type == ParticipantType.Creator);

                if (creator == null || creator.User.UserName != request.currentUserName)
                    return new Response<bool>("Bu rezervasyonu silme yetkiniz yok");

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response<bool>(true, "Rezervasyon silindi");
            }
        }
    }
}
