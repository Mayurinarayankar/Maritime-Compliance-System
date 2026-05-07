using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;
using MaritimeApp.Infrastructure.Data;
using MaritimeApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class MaintenanceTaskRepository : BaseRepository<MaintenanceTask>, IMaintenanceTaskRepository
{
    public MaintenanceTaskRepository(AppDbContext context) : base(context) { }

    public async Task<List<MaintenanceTask>> GetByShipAsync(Guid shipId) =>
        await _context.MaintenanceTasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Where(t => t.ShipId == shipId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<List<MaintenanceTask>> GetByAssigneeAsync(Guid userId) =>
        await _context.MaintenanceTasks
            .Include(t => t.Ship)
            .Include(t => t.CreatedBy)
            .Where(t => t.AssignedToId == userId)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

    public async Task<List<MaintenanceTask>> GetByStatusAsync(MaintenanceTaskStatus status) =>
        await _context.MaintenanceTasks
            .Include(t => t.Ship)
            .Include(t => t.AssignedTo)
            .Where(t => t.Status == status)
            .ToListAsync();

    public async Task<List<MaintenanceTask>> GetOverdueTasksAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.MaintenanceTasks
            .Include(t => t.Ship)
            .Include(t => t.AssignedTo)
            .Where(t => t.Status != MaintenanceTaskStatus.Completed && t.DueDate < now)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<MaintenanceTask?> GetWithDetailsAsync(Guid id) =>
        await _context.MaintenanceTasks
            .Include(t => t.Ship)
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Comments).ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<List<MaintenanceTask>> GetFilteredAsync(Guid? shipId, string? status, Guid? assignedToId)
    {
        var query = _context.MaintenanceTasks
            .Include(t => t.Ship)
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .AsQueryable();

        if (shipId.HasValue) query = query.Where(t => t.ShipId == shipId);
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<MaintenanceTaskStatus>(status, true, out var statusEnum))
            query = query.Where(t => t.Status == statusEnum);
        if (assignedToId.HasValue) query = query.Where(t => t.AssignedToId == assignedToId);

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }
}
