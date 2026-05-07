using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;
using MaritimeApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaritimeApp.Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
 
    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
 
    public async Task<List<User>> GetByShipAsync(Guid shipId) =>
        await _context.Users.Where(u => u.ShipId == shipId && u.IsActive).ToListAsync();
 
    public async Task<User?> GetWithDetailsAsync(Guid id) =>
        await _context.Users.Include(u => u.Ship).FirstOrDefaultAsync(u => u.Id == id);
 
    //public override async Task<T> AddAsync(User entity)
    public override async Task<User> AddAsync(User entity)
    {
        entity.Email = entity.Email.ToLower();
        return await base.AddAsync(entity);
    }
}