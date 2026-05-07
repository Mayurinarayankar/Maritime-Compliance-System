using MaritimeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaritimeApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ship> Ships => Set<Ship>();
    public DbSet<User> Users => Set<User>();
    public DbSet<MaintenanceTask> MaintenanceTasks => Set<MaintenanceTask>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();
    public DbSet<SafetyDrill> SafetyDrills => Set<SafetyDrill>();
    public DbSet<DrillAttendance> DrillAttendances => Set<DrillAttendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ship
        modelBuilder.Entity<Ship>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.IMONumber).IsRequired().HasMaxLength(20);
            e.HasIndex(x => x.IMONumber).IsUnique();
        });

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.Ship)
                .WithMany(s => s.CrewMembers)
                .HasForeignKey(x => x.ShipId)
                .OnDelete(DeleteBehavior.SetNull);
            e.Ignore(x => x.FullName);
        });

        // MaintenanceTask
        modelBuilder.Entity<MaintenanceTask>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(300);
            e.HasOne(x => x.Ship)
                .WithMany(s => s.MaintenanceTasks)
                .HasForeignKey(x => x.ShipId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(x => x.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            e.Ignore(x => x.IsOverdue);
        });

        // TaskComment
        modelBuilder.Entity<TaskComment>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // SafetyDrill
        modelBuilder.Entity<SafetyDrill>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(300);
            e.HasOne(x => x.Ship)
                .WithMany(s => s.SafetyDrills)
                .HasForeignKey(x => x.ShipId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            e.Ignore(x => x.IsMissed);
        });

        // DrillAttendance
        modelBuilder.Entity<DrillAttendance>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Drill)
                .WithMany(d => d.Attendances)
                .HasForeignKey(x => x.DrillId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.User)
                .WithMany(u => u.DrillAttendances)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.DrillId, x.UserId }).IsUnique();
        });
    }
}