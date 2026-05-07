using MaritimeApp.Domain.Common;

namespace MaritimeApp.Domain.Entities;
   public class Ship : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }
    public bool IsActive { get; set; } = true;
 
    public ICollection<MaintenanceTask> MaintenanceTasks { get; set; } = new List<MaintenanceTask>();
    public ICollection<SafetyDrill> SafetyDrills { get; set; } = new List<SafetyDrill>();
    public ICollection<User> CrewMembers { get; set; } = new List<User>();
}
