using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Application.IRepositories;

public interface IMaintenanceTaskRepository : IRepository<MaintenanceTask>
{
    Task<List<MaintenanceTask>> GetByShipAsync(Guid shipId);
    Task<List<MaintenanceTask>> GetByAssigneeAsync(Guid userId);
    Task<List<MaintenanceTask>> GetByStatusAsync(MaintenanceTaskStatus status);
    Task<List<MaintenanceTask>> GetOverdueTasksAsync();
    Task<MaintenanceTask?> GetWithDetailsAsync(Guid id);
    Task<List<MaintenanceTask>> GetFilteredAsync(Guid? shipId, string? status, Guid? assignedToId);
}