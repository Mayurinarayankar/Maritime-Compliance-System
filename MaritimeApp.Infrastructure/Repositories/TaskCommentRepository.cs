
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Infrastructure.Data;
using MaritimeApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class TaskCommentRepository : BaseRepository<TaskComment>, ITaskCommentRepository
{
    public TaskCommentRepository(AppDbContext context) : base(context) { }

    public async Task<List<TaskComment>> GetByTaskAsync(Guid taskId) =>
        await _context.TaskComments.Include(c => c.Author).Where(c => c.TaskId == taskId).OrderBy(c => c.CreatedAt).ToListAsync();
}