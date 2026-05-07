using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Common;
using MaritimeApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaritimeApp.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public virtual async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}