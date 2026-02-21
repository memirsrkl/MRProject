using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllMeetingUsersQuery : IRequest<List<AddUserDto>>
{
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public GetAllMeetingUsersQuery(DateTime date, DateTime startTime, DateTime endTime)
    {
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
                    request.EndTime > r.Reservation.StartTime
                )
                .Select(r => r.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var availableUsers = await _context.Users
                .Select(u => new AddUserDto
                {
                    Id = u.Id,
                    UserFullName = u.GetFullName(),
                    IsAvailable = !busyUserIds.Contains(u.Id) 
                })
                .ToListAsync(cancellationToken);

            return availableUsers;
        }
    }
}