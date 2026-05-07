using MaritimeApp.Application.DTOs;

namespace MaritimeApp.Application.Interfaces;
public interface IMaintenanceService
{
    Task<List<MaintenanceTaskDto>> GetAllTasksAsync(Guid? shipId = null, string? status = null, Guid? assignedToId = null);
    Task<MaintenanceTaskDto?> GetTaskByIdAsync(Guid id);
    Task<MaintenanceTaskDto> CreateTaskAsync(CreateMaintenanceTaskDto dto, Guid createdById);
    Task<MaintenanceTaskDto?> UpdateTaskAsync(Guid id, UpdateMaintenanceTaskDto dto);
    Task<MaintenanceTaskDto?> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusDto dto, Guid userId);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<TaskCommentDto> AddCommentAsync(Guid taskId, AddTaskCommentDto dto, Guid authorId);
    Task SyncOverdueStatusAsync();
}