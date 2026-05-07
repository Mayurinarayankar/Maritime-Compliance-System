using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.IRepositories;
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetByShipAsync(Guid shipId);
    Task<User?> GetWithDetailsAsync(Guid id);
}