using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class EventTaskService : GenericService<EventTask>, IEventTaskService
    {
        private readonly IEventTaskRepository _eventTaskRepository;

        public EventTaskService(IEventTaskRepository eventTaskRepository) : base(eventTaskRepository)
        {
            _eventTaskRepository = eventTaskRepository;
        }

        public async Task<IEnumerable<EventTask>> GetTasksByEventIdAsync(int eventId)
        {
            return await _eventTaskRepository.GetTasksByEventIdAsync(eventId);
        }

        public async Task<IEnumerable<EventTask>> GetTasksByPriorityAsync(TaskPriority priority)
        {
            return await _eventTaskRepository.GetTasksByPriorityAsync(priority);
        }

        public async Task<IEnumerable<EventTask>> GetOverdueTasksAsync()
        {
            return await _eventTaskRepository.GetOverdueTasksAsync();
        }

        public async Task<IEnumerable<EventTask>> GetTasksDueSoonAsync(int days = 7)
        {
            return await _eventTaskRepository.GetTasksDueSoonAsync(days);
        }

        public async Task<bool> MarkTaskAsCompletedAsync(int taskId)
        {
            return await _eventTaskRepository.MarkTaskAsCompletedAsync(taskId);
        }

        public override async Task<EventTask> CreateAsync(EventTask task)
        {
            if (task.CreatedAt == default(DateTime))
                task.CreatedAt = DateTime.UtcNow;
            if (task.TaskStatus == default(TaskStatuses))
                task.TaskStatus = TaskStatuses.Pending;
            return await base.CreateAsync(task);
        }

        public override async Task<EventTask> UpdateAsync(EventTask task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            return await base.UpdateAsync(task);
        }

        public async Task<EventTask> PartialUpdateAsync(int id, string title, string description, DateTime? dueDate, TaskPriority? priority, TaskStatuses? taskStatus, string assignedTo, bool? isCompleted)
        {
            var existingTask = await _eventTaskRepository.GetByIdAsync(id);
            if (existingTask == null) return null;

            if (!string.IsNullOrEmpty(title)) existingTask.Title = title;
            if (!string.IsNullOrEmpty(description)) existingTask.Description = description;
            if (dueDate.HasValue) existingTask.DueDate = dueDate.Value;
            if (priority.HasValue) existingTask.Priority = priority.Value;
            if (taskStatus.HasValue) existingTask.TaskStatus = taskStatus.Value;
            if (!string.IsNullOrEmpty(assignedTo)) existingTask.AssignedTo = assignedTo;
            if (isCompleted.HasValue) existingTask.IsCompleted = isCompleted.Value;

            return await UpdateAsync(existingTask);
        }

        public async Task<IEnumerable<EventTask>> GetTasksByStatusAsync(int status)
        {
            var allTasks = await _eventTaskRepository.GetAllAsync();
            return allTasks.Where(t => (int)t.TaskStatus == status);
        }

        public async Task<IEnumerable<EventTask>> GetPendingTasksAsync()
        {
            var allTasks = await _eventTaskRepository.GetAllAsync();
            return allTasks.Where(t => !t.IsCompleted);
        }

        public async Task<EventTask> CreateTaskFromDtoAsync(string title, string description, DateTime dueDate, TaskPriority priority, string assignedTo, int eventId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title is required");

            if (eventId <= 0)
                throw new ArgumentException("Valid event ID is required");

            var task = new EventTask
            {
                Title = title,
                Description = description ?? string.Empty,
                DueDate = dueDate,
                Priority = priority,
                AssignedTo = assignedTo ?? string.Empty,
                EventId = eventId,
                TaskStatus = TaskStatuses.Pending,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            return await base.CreateAsync(task);
        }
    }
}
