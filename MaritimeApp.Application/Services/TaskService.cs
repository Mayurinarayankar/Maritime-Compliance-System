using MaritimeApp.Domain.Entities;
using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;


namespace MaritimeApp.Application.Services;

public class TaskService : ITaskService
{
    private readonly IAppDbContext _context;

    public TaskService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task CreateTask(CreateTaskDto dto)
    {
        var task = new MaintenanceTask
        {
            Title = dto.Title,
            DueDate = dto.DueDate,
            CrewId = dto.CrewId,
            Status = "Pending"
        };

        await _context.AddTaskAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MaintenanceTask>> GetAllTasks()
    {
        return await _context.GetTasksAsync();
        // return await _context.Tasks.ToListAsync();
    }

    public async Task UpdateTask(int id, UpdateTaskDto dto)
    {
        var task = await _context.GetTaskByIdAsync(id);

        if (task == null)
            throw new Exception("Task not found");

        var validStatuses = new[] { "Pending", "InProgress", "Completed" };

        if (!validStatuses.Contains(dto.Status))
            throw new Exception("Invalid status");

        task.Status = dto.Status;

        await _context.SaveChangesAsync();
    }
}
