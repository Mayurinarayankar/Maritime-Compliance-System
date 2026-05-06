using MaritimeApp.Application.Interfaces;
using MaritimeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaritimeApp.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Ship> Ships { get; set; }
    public DbSet<Crew> Crews { get; set; }
    public DbSet<MaintenanceTask> Tasks { get; set; }
    public DbSet<Drill> Drills { get; set; }
    public DbSet<DrillParticipation> Participations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // ✅ Implement interface methods

    public async Task<List<MaintenanceTask>> GetTasksAsync()
    {
        return await Tasks.ToListAsync();
    }

    public async Task AddTaskAsync(MaintenanceTask task)
    {
        await Tasks.AddAsync(task);
    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
    public async Task<MaintenanceTask?> GetTaskByIdAsync(int id)
    {
        return await Tasks.FindAsync(id);
    }
}