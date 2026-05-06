namespace MaritimeApp.Domain.Entities;
public class DrillParticipation
{
    public int Id { get; set; }
    public int CrewId { get; set; }
    public int DrillId { get; set; }
    public bool Attended { get; set; }
}