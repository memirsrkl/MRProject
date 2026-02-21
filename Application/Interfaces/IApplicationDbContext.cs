using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Users> Users { get; }
    public DbSet<Reservation> Reservations { get; }
    public DbSet<MeetingRooms> MeetingRooms  { get; }
    public DbSet<ReservationUser> ReservationUsers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}