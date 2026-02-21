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
        modelBuilder.Entity<Reservation>().HasOne(t=>t.User).WithMany(t=>t.Reservations).HasForeignKey(t=>t.UserId);
        modelBuilder.Entity<ReservationUser>().HasOne(t => t.Reservation).WithMany(t => t.ReservationUsers).HasForeignKey(t => t.Id);
        modelBuilder.Entity<ReservationUser>().HasOne(t => t.Reservation).WithMany(t => t.ReservationUsers).HasForeignKey(t => t.UserId);
    }
}