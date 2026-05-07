using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.IRepositories;
public interface ISafetyDrillRepository : IRepository<SafetyDrill>
{
    Task<List<SafetyDrill>> GetByShipAsync(Guid shipId);
    Task<List<SafetyDrill>> GetUpcomingDrillsAsync();
    Task<List<SafetyDrill>> GetMissedDrillsAsync();
    Task<SafetyDrill?> GetWithDetailsAsync(Guid id);
    Task<List<SafetyDrill>> GetFilteredAsync(Guid? shipId, bool? isCompleted);
}