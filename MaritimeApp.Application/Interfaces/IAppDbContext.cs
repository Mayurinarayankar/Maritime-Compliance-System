using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.Interfaces;

public interface IAppDbContext
{
    Task<List<MaintenanceTask>> GetTasksAsync();
    Task AddTaskAsync(MaintenanceTask task);
    Task SaveChangesAsync();
    Task<MaintenanceTask?> GetTaskByIdAsync(int id);
}