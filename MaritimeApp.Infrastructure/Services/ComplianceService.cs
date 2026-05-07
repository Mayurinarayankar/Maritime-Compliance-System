using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Infrastructure.Services;

public class ComplianceService : IComplianceService
{
    private readonly IMaintenanceTaskRepository _taskRepo;
    private readonly ISafetyDrillRepository _drillRepo;
    private readonly IShipRepository _shipRepo;

    public ComplianceService(IMaintenanceTaskRepository taskRepo, ISafetyDrillRepository drillRepo, IShipRepository shipRepo)
    {
        _taskRepo = taskRepo;
        _drillRepo = drillRepo;
        _shipRepo = shipRepo;
    }

    public async Task<ComplianceDashboardDto> GetDashboardAsync(Guid? shipId = null)
    {
        var tasks = await _taskRepo.GetFilteredAsync(shipId, null, null);
        var drills = await _drillRepo.GetFilteredAsync(shipId, null);
        var now = DateTime.UtcNow;

        // Maintenance summary
        var maintenanceSummary = new MaintenanceSummaryDto
        {
            Total = tasks.Count,
            Pending = tasks.Count(t => t.Status == MaintenanceTaskStatus.Pending),
            InProgress = tasks.Count(t => t.Status == MaintenanceTaskStatus.InProgress),
            Completed = tasks.Count(t => t.Status == MaintenanceTaskStatus.Completed),
            Overdue = tasks.Count(t => t.Status != MaintenanceTaskStatus.Completed && t.DueDate < now),
            CompletionRate = tasks.Count > 0 ? Math.Round((double)tasks.Count(t => t.Status == MaintenanceTaskStatus.Completed) / tasks.Count * 100, 1) : 100
        };

        // Drill summary
        var drillSummary = new DrillSummaryDto
        {
            Total = drills.Count,
            Upcoming = drills.Count(d => !d.IsCompleted && d.ScheduledDate >= now),
            Completed = drills.Count(d => d.IsCompleted),
            Missed = drills.Count(d => !d.IsCompleted && d.ScheduledDate < now),
        };
        drillSummary.CompletionRate = drills.Count > 0
            ? Math.Round((double)drillSummary.Completed / drills.Count * 100, 1) : 100;
        drillSummary.AverageAttendanceRate = drills
            .Where(d => d.IsCompleted && d.Attendances.Any())
            .Select(d => d.Attendances.Count(a => a.Attended) * 100.0 / d.Attendances.Count)
            .DefaultIfEmpty(0)
            .Average();

        // Overall compliance
        var maintenanceRate = maintenanceSummary.CompletionRate;
        var drillRate = drillSummary.CompletionRate;
        var overall = Math.Round((maintenanceRate + drillRate) / 2, 1);
        var status = overall >= 80 ? "Compliant" : overall >= 60 ? "AtRisk" : "NonCompliant";

        // Per-ship compliance
        var ships = await _shipRepo.GetActiveShipsAsync();
        if (shipId.HasValue) ships = ships.Where(s => s.Id == shipId).ToList();
        var shipCompliances = new List<ShipComplianceDto>();
        foreach (var ship in ships)
            shipCompliances.Add(await GetShipComplianceAsync(ship.Id));

        // Overdue/missed for display
        var overdueTasks = tasks
            .Where(t => t.Status != MaintenanceTaskStatus.Completed && t.DueDate < now)
            .OrderBy(t => t.DueDate)
            .Take(10)
            .Select(t => new MaintenanceTaskDto
            {
                Id = t.Id, Title = t.Title, Status = t.Status.ToString(),
                DueDate = t.DueDate, Priority = t.Priority, IsOverdue = true,
                ShipId = t.ShipId, ShipName = t.Ship?.Name ?? string.Empty,
                AssignedToId = t.AssignedToId,
                AssignedToName = t.AssignedTo != null ? $"{t.AssignedTo.FirstName} {t.AssignedTo.LastName}" : null,
                CreatedById = t.CreatedById, CreatedAt = t.CreatedAt
            }).ToList();

        var missedDrills = drills
            .Where(d => !d.IsCompleted && d.ScheduledDate < now)
            .OrderBy(d => d.ScheduledDate)
            .Take(10)
            .Select(d => new SafetyDrillDto
            {
                Id = d.Id, Title = d.Title, DrillType = d.DrillType.ToString(),
                ScheduledDate = d.ScheduledDate, IsCompleted = false, IsMissed = true,
                ShipId = d.ShipId, ShipName = d.Ship?.Name ?? string.Empty,
                CreatedById = d.CreatedById, CreatedAt = d.CreatedAt
            }).ToList();

        return new ComplianceDashboardDto
        {
            Overall = new OverallComplianceDto
            {
                CompliancePercentage = overall,
                ComplianceStatus = status,
                MaintenanceComplianceRate = maintenanceRate,
                DrillComplianceRate = drillRate
            },
            Ships = shipCompliances,
            Maintenance = maintenanceSummary,
            Drills = drillSummary,
            OverdueTasks = overdueTasks,
            MissedDrills = missedDrills
        };
    }

    public async Task<ShipComplianceDto> GetShipComplianceAsync(Guid shipId)
    {
        var ship = await _shipRepo.GetByIdAsync(shipId);
        var tasks = await _taskRepo.GetFilteredAsync(shipId, null, null);
        var drills = await _drillRepo.GetFilteredAsync(shipId, null);
        var now = DateTime.UtcNow;

        var completedTasks = tasks.Count(t => t.Status == MaintenanceTaskStatus.Completed);
        var overdueTasks = tasks.Count(t => t.Status != MaintenanceTaskStatus.Completed && t.DueDate < now);
        var completedDrills = drills.Count(d => d.IsCompleted);
        var missedDrills = drills.Count(d => !d.IsCompleted && d.ScheduledDate < now);

        var taskRate = tasks.Count > 0 ? (double)completedTasks / tasks.Count * 100 : 100;
        var drillRate = drills.Count > 0 ? (double)completedDrills / drills.Count * 100 : 100;
        var score = Math.Round((taskRate + drillRate) / 2, 1);

        return new ShipComplianceDto
        {
            ShipId = shipId,
            ShipName = ship?.Name ?? string.Empty,
            ComplianceScore = score,
            Status = score >= 80 ? "Compliant" : score >= 60 ? "AtRisk" : "NonCompliant",
            TotalTasks = tasks.Count,
            CompletedTasks = completedTasks,
            OverdueTasks = overdueTasks,
            TotalDrills = drills.Count,
            CompletedDrills = completedDrills,
            MissedDrills = missedDrills
        };
    }
}