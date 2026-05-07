using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Infrastructure.Data;
using MaritimeApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class DrillAttendanceRepository : BaseRepository<DrillAttendance>, IDrillAttendanceRepository
{
    public DrillAttendanceRepository(AppDbContext context) : base(context) { }

    public async Task<List<DrillAttendance>> GetByDrillAsync(Guid drillId) =>
        await _context.DrillAttendances.Include(a => a.User).Where(a => a.DrillId == drillId).ToListAsync();

    public async Task<List<DrillAttendance>> GetByUserAsync(Guid userId) =>
        await _context.DrillAttendances.Include(a => a.Drill).Where(a => a.UserId == userId).ToListAsync();

    public async Task<DrillAttendance?> GetByDrillAndUserAsync(Guid drillId, Guid userId) =>
        await _context.DrillAttendances.FirstOrDefaultAsync(a => a.DrillId == drillId && a.UserId == userId);
}