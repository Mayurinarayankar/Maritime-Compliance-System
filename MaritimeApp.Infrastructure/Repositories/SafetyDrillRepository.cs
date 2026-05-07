using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Infrastructure.Data;
using MaritimeApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class SafetyDrillRepository : BaseRepository<SafetyDrill>, ISafetyDrillRepository
{
    public SafetyDrillRepository(AppDbContext context) : base(context) { }

    public async Task<List<SafetyDrill>> GetByShipAsync(Guid shipId) =>
        await _context.SafetyDrills
            .Include(d => d.CreatedBy)
            .Include(d => d.Attendances)
            .Where(d => d.ShipId == shipId)
            .OrderByDescending(d => d.ScheduledDate)
            .ToListAsync();

    public async Task<List<SafetyDrill>> GetUpcomingDrillsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.SafetyDrills
            .Include(d => d.Ship)
            .Where(d => !d.IsCompleted && d.ScheduledDate >= now)
            .OrderBy(d => d.ScheduledDate)
            .ToListAsync();
    }

    public async Task<List<SafetyDrill>> GetMissedDrillsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.SafetyDrills
            .Include(d => d.Ship)
            .Where(d => !d.IsCompleted && d.ScheduledDate < now)
            .OrderBy(d => d.ScheduledDate)
            .ToListAsync();
    }

    public async Task<SafetyDrill?> GetWithDetailsAsync(Guid id) =>
        await _context.SafetyDrills
            .Include(d => d.Ship)
            .Include(d => d.CreatedBy)
            .Include(d => d.Attendances).ThenInclude(a => a.User)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<List<SafetyDrill>> GetFilteredAsync(Guid? shipId, bool? isCompleted)
    {
        var query = _context.SafetyDrills
            .Include(d => d.Ship)
            .Include(d => d.CreatedBy)
            .Include(d => d.Attendances)
            .AsQueryable();

        if (shipId.HasValue) query = query.Where(d => d.ShipId == shipId);
        if (isCompleted.HasValue) query = query.Where(d => d.IsCompleted == isCompleted);

        return await query.OrderByDescending(d => d.ScheduledDate).ToListAsync();
    }
}
