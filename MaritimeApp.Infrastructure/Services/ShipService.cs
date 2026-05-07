using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Infrastructure.Services;

public class ShipService : IShipService
{
    private readonly IShipRepository _repo;

    public ShipService(IShipRepository repo) => _repo = repo;

    public async Task<List<ShipDto>> GetAllShipsAsync()
    {
        var ships = await _repo.GetAllAsync();
        return ships.Select(MapDto).ToList();
    }

    public async Task<ShipDto?> GetShipByIdAsync(Guid id)
    {
        var ship = await _repo.GetWithDetailsAsync(id);
        return ship == null ? null : MapDtoWithDetails(ship);
    }

    public async Task<ShipDto> CreateShipAsync(CreateShipDto dto)
    {
        var ship = new Ship
        {
            Name = dto.Name,
            IMONumber = dto.IMONumber,
            Type = dto.Type,
            Flag = dto.Flag,
            YearBuilt = dto.YearBuilt,
            IsActive = true
        };
        await _repo.AddAsync(ship);
        return MapDto(ship);
    }

    public async Task<ShipDto?> UpdateShipAsync(Guid id, UpdateShipDto dto)
    {
        var ship = await _repo.GetByIdAsync(id);
        if (ship == null) return null;
        ship.Name = dto.Name;
        ship.Type = dto.Type;
        ship.Flag = dto.Flag;
        ship.IsActive = dto.IsActive;
        await _repo.UpdateAsync(ship);
        return MapDto(ship);
    }

    public async Task<bool> DeleteShipAsync(Guid id) => await _repo.DeleteAsync(id);

    private static ShipDto MapDto(Ship s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        IMONumber = s.IMONumber,
        Type = s.Type,
        Flag = s.Flag,
        YearBuilt = s.YearBuilt,
        IsActive = s.IsActive,
        CrewCount = s.CrewMembers?.Count ?? 0,
        PendingTaskCount = s.MaintenanceTasks?.Count(t => t.Status == Domain.Enums.MaintenanceTaskStatus.Pending) ?? 0,
        UpcomingDrillCount = s.SafetyDrills?.Count(d => !d.IsCompleted && d.ScheduledDate >= DateTime.UtcNow) ?? 0
    };

    private static ShipDto MapDtoWithDetails(Ship s) => MapDto(s);
}