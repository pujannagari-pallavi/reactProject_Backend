using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Data.Data;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Implementations
{
    public class EventTaskRepository : GenericRepository<EventTask>, IEventTaskRepository
    {
        public EventTaskRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EventTask>> GetTasksByEventIdAsync(int eventId)
        {
            return await _dbSet.Where(t => t.EventId == eventId).OrderBy(t => t.DueDate).ToListAsync();
        }

        public async Task<IEnumerable<EventTask>> GetTasksByPriorityAsync(TaskPriority priority)
        {
            return await _dbSet.Where(t => t.Priority == priority).ToListAsync();
        }

        public async Task<IEnumerable<EventTask>> GetOverdueTasksAsync()
        {
            return await _dbSet.Where(t => t.DueDate < DateTime.UtcNow && !t.IsCompleted).ToListAsync();
        }

        public async Task<IEnumerable<EventTask>> GetTasksDueSoonAsync(int days = 7)
        {
            var dueDate = DateTime.UtcNow.AddDays(days);
            return await _dbSet.Where(t => t.DueDate <= dueDate && !t.IsCompleted).ToListAsync();
        }

        public async Task<bool> MarkTaskAsCompletedAsync(int taskId)
        {
            var task = await GetByIdAsync(taskId);
            if (task == null) return false;

            task.IsCompleted = true;
            task.CompletedAt = DateTime.UtcNow;
            task.TaskStatus = TaskStatuses.Completed;
            await UpdateAsync(task);
            return true;
        }
    }
}
