using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Interfaces
{
    public interface IEventTaskRepository : IGenericRepository<EventTask>
    {
        Task<IEnumerable<EventTask>> GetTasksByEventIdAsync(int eventId);
        Task<IEnumerable<EventTask>> GetTasksByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<EventTask>> GetOverdueTasksAsync();
        Task<IEnumerable<EventTask>> GetTasksDueSoonAsync(int days = 7);
        Task<bool> MarkTaskAsCompletedAsync(int taskId);
    }
}
