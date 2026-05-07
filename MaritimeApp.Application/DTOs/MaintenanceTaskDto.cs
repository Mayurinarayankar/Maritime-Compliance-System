namespace MaritimeApp.Application.DTOs;

public class MaintenanceTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Priority { get; set; } = string.Empty;
    public bool IsOverdue { get; set; }
    public Guid ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public Guid? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public Guid CreatedById { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<TaskCommentDto> Comments { get; set; } = new();
}

public class CreateMaintenanceTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Priority { get; set; } = "Medium";
    public Guid ShipId { get; set; }
    public Guid? AssignedToId { get; set; }
}

public class UpdateMaintenanceTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Priority { get; set; } = string.Empty;
    public Guid? AssignedToId { get; set; }
}

public class UpdateTaskStatusDto
{
    public string Status { get; set; } = string.Empty;
    public string? Comment { get; set; }
}

public class TaskCommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AddTaskCommentDto
{
    public string Content { get; set; } = string.Empty;
}