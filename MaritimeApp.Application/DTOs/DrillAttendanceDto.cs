namespace MaritimeApp.Application.DTOs
{
    public class DrillAttendanceDto
    {
        public int CrewId { get; set; }
        public int DrillId { get; set; }
        public bool Attended { get; set; }
    }
}