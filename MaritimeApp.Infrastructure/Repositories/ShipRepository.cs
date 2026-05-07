using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Infrastructure.Data;
using MaritimeApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ShipRepository : BaseRepository<Ship>, IShipRepository
{
    public ShipRepository(AppDbContext context) : base(context) { }

    public async Task<Ship?> GetWithDetailsAsync(Guid id) =>
        await _context.Ships
            .Include(s => s.CrewMembers)
            .Include(s => s.MaintenanceTasks)
            .Include(s => s.SafetyDrills)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<List<Ship>> GetActiveShipsAsync() =>
        await _context.Ships.Where(s => s.IsActive).ToListAsync();
}