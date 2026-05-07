using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.Interfaces;

public interface IAppDbContext
{
    Task<List<MaintenanceTask>> GetTasksAsync();
    Task AddTaskAsync(MaintenanceTask task);
    Task SaveChangesAsync();
    Task<MaintenanceTask?> GetTaskByIdAsync(int id);

    // Drill related methods
    Task AddDrillAsync(Drill drill);

    Task<List<Drill>> GetDrillsAsync();

    Task<Drill?> GetDrillByIdAsync(int id);

    Task AddParticipationAsync(DrillParticipation participation);

    Task<List<DrillParticipation>> GetParticipationsAsync();
    Task<Ship?> GetShipByIdAsync(int shipId);

}