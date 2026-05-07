using MaritimeApp.Domain.Common;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Crew;
    public string Rank { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Guid? ShipId { get; set; }
    public Ship? Ship { get; set; }

    public ICollection<MaintenanceTask> AssignedTasks { get; set; } = new List<MaintenanceTask>();
    public ICollection<MaintenanceTask> CreatedTasks { get; set; } = new List<MaintenanceTask>();
    public ICollection<DrillAttendance> DrillAttendances { get; set; } = new List<DrillAttendance>();
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();

    public string FullName => $"{FirstName} {LastName}";
}