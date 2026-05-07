using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.IRepositories;
 
public interface IDrillAttendanceRepository : IRepository<DrillAttendance>
{
    Task<List<DrillAttendance>> GetByDrillAsync(Guid drillId);
    Task<List<DrillAttendance>> GetByUserAsync(Guid userId);
    Task<DrillAttendance?> GetByDrillAndUserAsync(Guid drillId, Guid userId);
}