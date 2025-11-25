using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IEventTaskService : IGenericService<EventTask>
    {
        Task<IEnumerable<EventTask>> GetTasksByEventIdAsync(int eventId);
        Task<IEnumerable<EventTask>> GetTasksByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<EventTask>> GetOverdueTasksAsync();
        Task<IEnumerable<EventTask>> GetTasksDueSoonAsync(int days = 7);
        Task<bool> MarkTaskAsCompletedAsync(int taskId);
        Task<EventTask> PartialUpdateAsync(int id, string title, string description, DateTime? dueDate, TaskPriority? priority, TaskStatuses? taskStatus, string assignedTo, bool? isCompleted);
        Task<IEnumerable<EventTask>> GetTasksByStatusAsync(int status);
        Task<IEnumerable<EventTask>> GetPendingTasksAsync();
        Task<EventTask> CreateTaskFromDtoAsync(string title, string description, DateTime dueDate, TaskPriority priority, string assignedTo, int eventId);
    }
}
