using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.IRepositories;

public interface IShipRepository : IRepository<Ship>
{
    Task<Ship?> GetWithDetailsAsync(Guid id);
    Task<List<Ship>> GetActiveShipsAsync();
}