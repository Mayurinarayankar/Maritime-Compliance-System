namespace MaritimeApp.Application.DTOs;

public class ComplianceDashboardDto
{
    public OverallComplianceDto Overall { get; set; } = new();
    public List<ShipComplianceDto> Ships { get; set; } = new();
    public MaintenanceSummaryDto Maintenance { get; set; } = new();
    public DrillSummaryDto Drills { get; set; } = new();
    public List<MaintenanceTaskDto> OverdueTasks { get; set; } = new();
    public List<SafetyDrillDto> MissedDrills { get; set; } = new();
}

public class OverallComplianceDto
{
    public double CompliancePercentage { get; set; }
    public string ComplianceStatus { get; set; } = string.Empty; // Compliant, AtRisk, NonCompliant
    public double MaintenanceComplianceRate { get; set; }
    public double DrillComplianceRate { get; set; }
}

public class ShipComplianceDto
{
    public Guid ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public double ComplianceScore { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OverdueTasks { get; set; }
    public int TotalDrills { get; set; }
    public int CompletedDrills { get; set; }
    public int MissedDrills { get; set; }
}

public class MaintenanceSummaryDto
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int InProgress { get; set; }
    public int Completed { get; set; }
    public int Overdue { get; set; }
    public double CompletionRate { get; set; }
}

public class DrillSummaryDto
{
    public int Total { get; set; }
    public int Upcoming { get; set; }
    public int Completed { get; set; }
    public int Missed { get; set; }
    public double CompletionRate { get; set; }
    public double AverageAttendanceRate { get; set; }
}