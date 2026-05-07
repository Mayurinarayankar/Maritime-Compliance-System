using MaritimeApp.Domain.Common;

namespace MaritimeApp.Domain.Entities;

public class DrillAttendance : BaseEntity
{
    public bool Attended { get; set; } = false;
    public DateTime? MarkedAt { get; set; }
    public string? Notes { get; set; }

    public Guid DrillId { get; set; }
    public SafetyDrill Drill { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}