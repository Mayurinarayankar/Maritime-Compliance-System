using MaritimeApp.Domain.Entities;

public interface IShipRepository
{
    Task<Ship?> GetByIdAsync(int id);
}