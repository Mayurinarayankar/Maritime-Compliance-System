using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.Services;

public class DrillService : IDrillService
{
    private readonly IAppDbContext _context;

    public DrillService(IAppDbContext context)
    {
        _context = context;
    }

  public async Task ScheduleDrill(CreateDrillDto dto)
{
   var ship = await _context.GetShipByIdAsync(dto.ShipId);

if (ship == null)
    throw new Exception($"Ship with Id {dto.ShipId} does not exist");
    var drill = new Drill
    {
        DrillType = dto.DrillType,
        ScheduledDate = dto.ScheduledDate,
        ShipId = dto.ShipId,
        IsCompleted = false
    };

    await _context.AddDrillAsync(drill);
    await _context.SaveChangesAsync();
}

    public async Task<List<Drill>> GetAllDrills()
    {
        return await _context.GetDrillsAsync();
    }

    public async Task MarkAttendance(DrillAttendanceDto dto)
    {
        var participation = new DrillParticipation
        {
            CrewId = dto.CrewId,
            DrillId = dto.DrillId,
            Attended = dto.Attended
        };

        await _context.AddParticipationAsync(participation);
        await _context.SaveChangesAsync();
    }
}