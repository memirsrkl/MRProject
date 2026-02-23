using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.Extensions;
using Domain;
namespace Application.Features.Reservations.Query
{
    public class GetRoomReservationQuery : IRequest<List<ReservationDTO>>
    {
        public Guid roomId { get; set; }
        public GetRoomReservationQuery(Guid roomId)
        {
            this.roomId = roomId;
        }
        public class GetRoomReservationQueryHandler : IRequestHandler<GetRoomReservationQuery, List<ReservationDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetRoomReservationQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<ReservationDTO>> Handle(GetRoomReservationQuery request, CancellationToken cancellationToken)
            {
                var reservations = await _context.Reservations
    .Where(r => r.MeetingRoomId == request.roomId && !r.IsDeleted && r.Status!=Enums.ReservationStatus.Rejected)
    .ToListAsync(cancellationToken);
                var events = reservations.Select(r => new ReservationDTO
                {
                    Title = r.Status.GetName(),
                    Start = r.StartTime,
                    End = r.EndTime,
                    BackgroundColor = r.Status.GetColor(),
                    BorderColor = r.Status.GetColor(),
                    TextColor = "#fff"
                }).ToList();
                return events;
            }
        }
    }
}
