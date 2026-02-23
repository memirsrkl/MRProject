using Application.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllMeetingUsersQuery : IRequest<List<AddUserDto>>
{
    public Guid? MeetingId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public GetAllMeetingUsersQuery(Guid? meetingId, DateTime date, DateTime startTime, DateTime endTime)
    {
        MeetingId = meetingId;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public class Handler : IRequestHandler<GetAllMeetingUsersQuery, List<AddUserDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AddUserDto>> Handle(
            GetAllMeetingUsersQuery request,
            CancellationToken cancellationToken)
        {
            var busyUserIds = await _context.ReservationUsers
        .Include(r => r.Reservation)
        .Where(r =>
            r.Reservation.StartTime.Date == request.Date.Date &&
            request.StartTime < r.Reservation.EndTime &&
            request.EndTime > r.Reservation.StartTime &&
            r.Reservation.Status != Enums.ReservationStatus.Rejected
        )
        .Select(r => r.UserId)
        .Distinct()
        .ToListAsync(cancellationToken);

            var existingUserIds = await _context.ReservationUsers
                .Where(r => r.ReservationId == request.MeetingId)
                .Select(r => r.UserId)
                .ToListAsync(cancellationToken);

            var availableUsers = await _context.Users
                .Where(u =>
                    !busyUserIds.Contains(u.Id) &&
                    !existingUserIds.Contains(u.Id)
                )
                .Select(u => new AddUserDto
                {
                    Id = u.Id,
                    UserFullName = u.GetFullName(),
                    IsAvailable = true
                })
                .ToListAsync(cancellationToken);

            return availableUsers;
        }
    }
}