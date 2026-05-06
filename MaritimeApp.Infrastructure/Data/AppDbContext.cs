using MaritimeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaritimeApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Ship> Ships { get; set; }
    public DbSet<Crew> Crews { get; set; }
    public DbSet<MaintenanceTask> Tasks { get; set; }
    public DbSet<Drill> Drills { get; set; }
    public DbSet<DrillParticipation> Participations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}