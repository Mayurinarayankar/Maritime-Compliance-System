using MaritimeApp.Application.DTOs;

namespace MaritimeApp.Application.Interfaces;

public interface IShipService
{
    Task<List<ShipDto>> GetAllShipsAsync();
    Task<ShipDto?> GetShipByIdAsync(Guid id);
    Task<ShipDto> CreateShipAsync(CreateShipDto dto);
    Task<ShipDto?> UpdateShipAsync(Guid id, UpdateShipDto dto);
    Task<bool> DeleteShipAsync(Guid id);
}