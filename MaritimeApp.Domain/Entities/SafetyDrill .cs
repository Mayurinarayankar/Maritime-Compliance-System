using MaritimeApp.Domain.Common;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Domain.Entities;

public class SafetyDrill : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DrillType DrillType { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; } = false;
    public string? Notes { get; set; }

    public Guid ShipId { get; set; }
    public Ship Ship { get; set; } = null!;

    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public ICollection<DrillAttendance> Attendances { get; set; } = new List<DrillAttendance>();

    public bool IsMissed => !IsCompleted && ScheduledDate < DateTime.UtcNow;
}