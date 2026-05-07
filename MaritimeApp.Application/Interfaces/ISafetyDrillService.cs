using MaritimeApp.Application.DTOs;

namespace MaritimeApp.Application.Interfaces;
public interface ISafetyDrillService
{
    Task<List<SafetyDrillDto>> GetAllDrillsAsync(Guid? shipId = null, bool? isCompleted = null);
    Task<SafetyDrillDto?> GetDrillByIdAsync(Guid id, Guid? currentUserId = null);
    Task<SafetyDrillDto> CreateDrillAsync(CreateSafetyDrillDto dto, Guid createdById);
    Task<SafetyDrillDto?> UpdateDrillAsync(Guid id, UpdateSafetyDrillDto dto);
    Task<SafetyDrillDto?> CompleteDrillAsync(Guid id, CompleteDrillDto dto);
    Task<bool> DeleteDrillAsync(Guid id);
    Task<DrillAttendanceDto> MarkAttendanceAsync(Guid drillId, Guid userId, MarkAttendanceDto dto);
    Task<List<DrillAttendanceDto>> GetDrillAttendanceAsync(Guid drillId);
}