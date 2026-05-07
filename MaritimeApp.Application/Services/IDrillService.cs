using MaritimeApp.Application.DTOs;
using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.Services;

public interface IDrillService
{
    Task ScheduleDrill(CreateDrillDto dto);

    Task<List<Drill>> GetAllDrills();

    Task MarkAttendance(DrillAttendanceDto dto);
   
}