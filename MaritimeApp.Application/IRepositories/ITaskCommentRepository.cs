using MaritimeApp.Domain.Entities;

namespace MaritimeApp.Application.IRepositories;
 
public interface ITaskCommentRepository : IRepository<TaskComment>
{
    Task<List<TaskComment>> GetByTaskAsync(Guid taskId);
}