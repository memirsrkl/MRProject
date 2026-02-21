using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Users> Users => Set<Users>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<MeetingRooms> MeetingRooms => Set<MeetingRooms>();
    public DbSet<ReservationUser> ReservationUsers => Set<ReservationUser>();


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<ReservationUser>()
            .HasOne(ru => ru.Reservation)
            .WithMany(r => r.ReservationUsers)
            .HasForeignKey(ru => ru.ReservationId);

        modelBuilder.Entity<ReservationUser>()
            .HasOne(ru => ru.User)
            .WithMany(u => u.ReservationUser)
            .HasForeignKey(ru => ru.UserId);

        modelBuilder.Entity<ReservationUser>()
            .HasOne(ru => ru.MeetingRooms)
            .WithMany(m => m.ReservationUsers)
            .HasForeignKey(ru => ru.MeetingRoomId);
    }
}