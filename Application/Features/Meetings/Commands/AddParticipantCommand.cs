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
                ReservationUser user = new ReservationUser();
                user.ReservationId = request.model.MeetingId;
                user.UserId = request.model.UserId;
                user.CreatedTime = DateTime.Now;
                user.Type = Enums.ParticipantType.Participant;
                user.MeetingRoomId = reservation.MeetingRoomId;
                _context.ReservationUsers.Add(user);
                _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>(true);
            }
        }
    }
}
