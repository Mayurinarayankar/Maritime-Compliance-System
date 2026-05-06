namespace MaritimeApp.Domain.Entities;
public class MaintenanceTask
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } // Pending, InProgress, Completed
    public int CrewId { get; set; }
    public bool IsOverdue => Status != "Completed" && DueDate < DateTime.UtcNow;
}