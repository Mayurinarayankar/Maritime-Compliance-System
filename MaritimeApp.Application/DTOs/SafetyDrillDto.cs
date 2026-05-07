namespace MaritimeApp.Application.DTOs;

public class SafetyDrillDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DrillType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsMissed { get; set; }
    public string? Notes { get; set; }
    public Guid ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public Guid CreatedById { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TotalAttendees { get; set; }
    public int PresentCount { get; set; }
    public double AttendanceRate { get; set; }
    public bool? CurrentUserAttended { get; set; }
}

public class CreateSafetyDrillDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DrillType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public Guid ShipId { get; set; }
}

public class UpdateSafetyDrillDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string? Notes { get; set; }
}

public class DrillAttendanceDto
{
    public Guid Id { get; set; }
    public Guid DrillId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool Attended { get; set; }
    public DateTime? MarkedAt { get; set; }
    public string? Notes { get; set; }
}

public class MarkAttendanceDto
{
    public bool Attended { get; set; }
    public string? Notes { get; set; }
}

public class CompleteDrillDto
{
    public string? Notes { get; set; }
}