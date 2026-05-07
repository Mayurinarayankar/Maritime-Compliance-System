using MaritimeApp.Domain.Common;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Domain.Entities;

public class MaintenanceTask : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MaintenanceTaskStatus Status { get; set; } = MaintenanceTaskStatus.Pending;
    public DateTime DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

    public Guid ShipId { get; set; }
    public Ship Ship { get; set; } = null!;

    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();

    public bool IsOverdue => Status != MaintenanceTaskStatus.Completed && DueDate < DateTime.UtcNow;
}