using MaritimeApp.Domain.Common;

namespace MaritimeApp.Domain.Entities;

public class TaskComment : BaseEntity
{
    public string Content { get; set; } = string.Empty;

    public Guid TaskId { get; set; }
    public MaintenanceTask Task { get; set; } = null!;

    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
}