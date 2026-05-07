namespace MaritimeApp.Application.DTOs
{
    public class CreateDrillDto
    {
        public string DrillType { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int ShipId { get; set; }
    }
}