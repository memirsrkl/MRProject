using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservations.Command
{
    public class UpdateReservationStatusCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string? RejectDescription { get; set; }
        public UpdateReservationStatusCommand(Guid id, string status, string? rejectDescription)
        {
            Id = id;
            Status = status;
            RejectDescription = rejectDescription;
        }
        public class UpdateReservationStatusCommandHandler : IRequestHandler<UpdateReservationStatusCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateReservationStatusCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<bool>> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
            {
                var reservation = _context.Reservations.FirstOrDefault(t => t.Id == request.Id);
                if (reservation != null)
                {
                    reservation.Status = (Domain.Enums.ReservationStatus)Enum.Parse(typeof(Domain.Enums.ReservationStatus), request.Status);
                    reservation.RejectDescription = request.RejectDescription;
                    await _context.SaveChangesAsync(cancellationToken);
                    return new Response<bool>(true);
                }
                else
                {
                    return new Response<bool>(false);
                }
            }
        }
    }
}
