namespace MaritimeApp.Domain.Entities;
public class Drill
{
    public int Id { get; set; }

    public string DrillType { get; set; }

    public DateTime ScheduledDate { get; set; }

    public int ShipId { get; set; }

    public bool IsCompleted { get; set; }

    // Navigation Property
    public Ship Ship { get; set; }
}