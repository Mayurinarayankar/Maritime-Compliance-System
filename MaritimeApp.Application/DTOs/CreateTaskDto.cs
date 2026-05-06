namespace MaritimeApp.Application.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
    public int CrewId { get; set; }
}