namespace MaritimeApp.Application.DTOs;

public class ShipDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }
    public bool IsActive { get; set; }
    public int CrewCount { get; set; }
    public int PendingTaskCount { get; set; }
    public int UpcomingDrillCount { get; set; }
}

public class CreateShipDto
{
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }
}

public class UpdateShipDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}