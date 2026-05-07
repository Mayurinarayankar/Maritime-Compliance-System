using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Infrastructure.Services;

public class SafetyDrillService : ISafetyDrillService
{
    private readonly ISafetyDrillRepository _repo;
    private readonly IDrillAttendanceRepository _attendanceRepo;

    public SafetyDrillService(ISafetyDrillRepository repo, IDrillAttendanceRepository attendanceRepo)
    {
        _repo = repo;
        _attendanceRepo = attendanceRepo;
    }

    public async Task<List<SafetyDrillDto>> GetAllDrillsAsync(Guid? shipId = null, bool? isCompleted = null)
    {
        var drills = await _repo.GetFilteredAsync(shipId, isCompleted);
        return drills.Select(d => MapDto(d, null)).ToList();
    }

    public async Task<SafetyDrillDto?> GetDrillByIdAsync(Guid id, Guid? currentUserId = null)
    {
        var drill = await _repo.GetWithDetailsAsync(id);
        if (drill == null) return null;
        bool? attended = null;
        if (currentUserId.HasValue)
        {
            var attendance = await _attendanceRepo.GetByDrillAndUserAsync(id, currentUserId.Value);
            attended = attendance?.Attended;
        }
        return MapDto(drill, attended);
    }

    public async Task<SafetyDrillDto> CreateDrillAsync(CreateSafetyDrillDto dto, Guid createdById)
    {
        if (!Enum.TryParse<DrillType>(dto.DrillType, true, out var drillType))
            drillType = DrillType.FireDrill;

        var drill = new SafetyDrill
        {
            Title = dto.Title,
            Description = dto.Description,
            DrillType = drillType,
            ScheduledDate = dto.ScheduledDate,
            ShipId = dto.ShipId,
            CreatedById = createdById,
            IsCompleted = false
        };
        await _repo.AddAsync(drill);
        var full = await _repo.GetWithDetailsAsync(drill.Id);
        return MapDto(full!, null);
    }

    public async Task<SafetyDrillDto?> UpdateDrillAsync(Guid id, UpdateSafetyDrillDto dto)
    {
        var drill = await _repo.GetByIdAsync(id);
        if (drill == null) return null;
        drill.Title = dto.Title;
        drill.Description = dto.Description;
        drill.ScheduledDate = dto.ScheduledDate;
        drill.Notes = dto.Notes;
        await _repo.UpdateAsync(drill);
        var full = await _repo.GetWithDetailsAsync(id);
        return MapDto(full!, null);
    }

    public async Task<SafetyDrillDto?> CompleteDrillAsync(Guid id, CompleteDrillDto dto)
    {
        var drill = await _repo.GetByIdAsync(id);
        if (drill == null) return null;
        drill.IsCompleted = true;
        drill.CompletedAt = DateTime.UtcNow;
        drill.Notes = dto.Notes ?? drill.Notes;
        await _repo.UpdateAsync(drill);
        var full = await _repo.GetWithDetailsAsync(id);
        return MapDto(full!, null);
    }

    public async Task<bool> DeleteDrillAsync(Guid id) => await _repo.DeleteAsync(id);

    public async Task<DrillAttendanceDto> MarkAttendanceAsync(Guid drillId, Guid userId, MarkAttendanceDto dto)
    {
        var existing = await _attendanceRepo.GetByDrillAndUserAsync(drillId, userId);
        DrillAttendance attendance;

        if (existing != null)
        {
            existing.Attended = dto.Attended;
            existing.Notes = dto.Notes;
            existing.MarkedAt = DateTime.UtcNow;
            await _attendanceRepo.UpdateAsync(existing);
            attendance = existing;
        }
        else
        {
            attendance = new DrillAttendance
            {
                DrillId = drillId,
                UserId = userId,
                Attended = dto.Attended,
                Notes = dto.Notes,
                MarkedAt = DateTime.UtcNow
            };
            await _attendanceRepo.AddAsync(attendance);
        }

        return new DrillAttendanceDto
        {
            Id = attendance.Id,
            DrillId = attendance.DrillId,
            UserId = attendance.UserId,
            UserName = string.Empty,
            Attended = attendance.Attended,
            MarkedAt = attendance.MarkedAt,
            Notes = attendance.Notes
        };
    }

    public async Task<List<DrillAttendanceDto>> GetDrillAttendanceAsync(Guid drillId)
    {
        var attendances = await _attendanceRepo.GetByDrillAsync(drillId);
        return attendances.Select(a => new DrillAttendanceDto
        {
            Id = a.Id,
            DrillId = a.DrillId,
            UserId = a.UserId,
            UserName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : string.Empty,
            Attended = a.Attended,
            MarkedAt = a.MarkedAt,
            Notes = a.Notes
        }).ToList();
    }

    private static SafetyDrillDto MapDto(SafetyDrill d, bool? currentUserAttended)
    {
        var total = d.Attendances?.Count ?? 0;
        var present = d.Attendances?.Count(a => a.Attended) ?? 0;
        return new SafetyDrillDto
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            DrillType = d.DrillType.ToString(),
            ScheduledDate = d.ScheduledDate,
            CompletedAt = d.CompletedAt,
            IsCompleted = d.IsCompleted,
            IsMissed = !d.IsCompleted && d.ScheduledDate < DateTime.UtcNow,
            Notes = d.Notes,
            ShipId = d.ShipId,
            ShipName = d.Ship?.Name ?? string.Empty,
            CreatedById = d.CreatedById,
            CreatedByName = d.CreatedBy != null ? $"{d.CreatedBy.FirstName} {d.CreatedBy.LastName}" : string.Empty,
            CreatedAt = d.CreatedAt,
            TotalAttendees = total,
            PresentCount = present,
            AttendanceRate = total > 0 ? Math.Round((double)present / total * 100, 1) : 0,
            CurrentUserAttended = currentUserAttended
        };
    }
}