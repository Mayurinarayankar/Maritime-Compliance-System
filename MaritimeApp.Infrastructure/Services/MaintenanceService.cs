using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Infrastructure.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly IMaintenanceTaskRepository _repo;
    private readonly ITaskCommentRepository _commentRepo;
    private readonly IShipRepository _shipRepo;

    public MaintenanceService(IMaintenanceTaskRepository repo, ITaskCommentRepository commentRepo, IShipRepository shipRepo)
    {
        _repo = repo;
        _commentRepo = commentRepo;
        _shipRepo = shipRepo;
    }

    public async Task<List<MaintenanceTaskDto>> GetAllTasksAsync(Guid? shipId = null, string? status = null, Guid? assignedToId = null)
    {
        var tasks = await _repo.GetFilteredAsync(shipId, status, assignedToId);
        return tasks.Select(MapDto).ToList();
    }

    public async Task<MaintenanceTaskDto?> GetTaskByIdAsync(Guid id)
    {
        var task = await _repo.GetWithDetailsAsync(id);
        return task == null ? null : MapDtoWithComments(task);
    }

    public async Task<MaintenanceTaskDto> CreateTaskAsync(CreateMaintenanceTaskDto dto, Guid createdById)
    {
        var task = new MaintenanceTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            ShipId = dto.ShipId,
            AssignedToId = dto.AssignedToId,
            CreatedById = createdById,
            Status = MaintenanceTaskStatus.Pending
        };
        await _repo.AddAsync(task);
        var full = await _repo.GetWithDetailsAsync(task.Id);
        return MapDtoWithComments(full!);
    }

    public async Task<MaintenanceTaskDto?> UpdateTaskAsync(Guid id, UpdateMaintenanceTaskDto dto)
    {
        var task = await _repo.GetByIdAsync(id);
        if (task == null) return null;
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.Priority = dto.Priority;
        task.AssignedToId = dto.AssignedToId;
        if (Enum.TryParse<MaintenanceTaskStatus>(dto.Status, true, out var status))
            task.Status = status;
        await _repo.UpdateAsync(task);
        var full = await _repo.GetWithDetailsAsync(id);
        return MapDtoWithComments(full!);
    }

    public async Task<MaintenanceTaskDto?> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusDto dto, Guid userId)
    {
        var task = await _repo.GetWithDetailsAsync(id);
        if (task == null) return null;

        if (Enum.TryParse<MaintenanceTaskStatus>(dto.Status, true, out var status))
        {
            task.Status = status;
            if (status == MaintenanceTaskStatus.Completed)
                task.CompletedAt = DateTime.UtcNow;
        }

        if (!string.IsNullOrEmpty(dto.Comment))
        {
            var comment = new TaskComment { Content = dto.Comment, TaskId = id, AuthorId = userId };
            await _commentRepo.AddAsync(comment);
        }

        await _repo.UpdateAsync(task);
        var full = await _repo.GetWithDetailsAsync(id);
        return MapDtoWithComments(full!);
    }

    public async Task<bool> DeleteTaskAsync(Guid id) => await _repo.DeleteAsync(id);

    public async Task<TaskCommentDto> AddCommentAsync(Guid taskId, AddTaskCommentDto dto, Guid authorId)
    {
        var comment = new TaskComment { Content = dto.Content, TaskId = taskId, AuthorId = authorId };
        await _commentRepo.AddAsync(comment);
        var full = await _commentRepo.GetByTaskAsync(taskId);
        var added = full.Last();
        return new TaskCommentDto
        {
            Id = added.Id,
            Content = added.Content,
            AuthorId = added.AuthorId,
            AuthorName = added.Author != null ? $"{added.Author.FirstName} {added.Author.LastName}" : "Unknown",
            CreatedAt = added.CreatedAt
        };
    }

    public async Task SyncOverdueStatusAsync()
    {
        var overdue = await _repo.GetOverdueTasksAsync();
        foreach (var task in overdue.Where(t => t.Status != MaintenanceTaskStatus.Overdue))
        {
            task.Status = MaintenanceTaskStatus.Overdue;
            await _repo.UpdateAsync(task);
        }
    }

    private static MaintenanceTaskDto MapDto(MaintenanceTask t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status.ToString(),
        DueDate = t.DueDate,
        CompletedAt = t.CompletedAt,
        Priority = t.Priority,
        IsOverdue = t.Status != MaintenanceTaskStatus.Completed && t.DueDate < DateTime.UtcNow,
        ShipId = t.ShipId,
        ShipName = t.Ship?.Name ?? string.Empty,
        AssignedToId = t.AssignedToId,
        AssignedToName = t.AssignedTo != null ? $"{t.AssignedTo.FirstName} {t.AssignedTo.LastName}" : null,
        CreatedById = t.CreatedById,
        CreatedByName = t.CreatedBy != null ? $"{t.CreatedBy.FirstName} {t.CreatedBy.LastName}" : string.Empty,
        CreatedAt = t.CreatedAt,
        Comments = new List<TaskCommentDto>()
    };

    private static MaintenanceTaskDto MapDtoWithComments(MaintenanceTask t)
    {
        var dto = MapDto(t);
        dto.Comments = t.Comments?.Select(c => new TaskCommentDto
        {
            Id = c.Id,
            Content = c.Content,
            AuthorId = c.AuthorId,
            AuthorName = c.Author != null ? $"{c.Author.FirstName} {c.Author.LastName}" : "Unknown",
            CreatedAt = c.CreatedAt
        }).ToList() ?? new List<TaskCommentDto>();
        return dto;
    }
}