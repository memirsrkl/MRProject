using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
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
        public RemoveParticipantCommand(MettingAddParticipant model)
        {
            this.model = model;
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
                var reservationUser = _context.ReservationUsers.Where(t => t.Id == request.model.UserId).FirstOrDefault();
                _context.ReservationUsers.Remove(reservationUser);
                _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>(true);
            }
        }
    }
}
