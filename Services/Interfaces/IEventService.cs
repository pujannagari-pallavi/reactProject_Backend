using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IEventService : IGenericService<Event>
    {
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId);
        Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Event> GetEventWithDetailsAsync(int eventId);
        Task UpdatePastEventsStatusAsync();
        Task<Event> PartialUpdateAsync(int id, string title, EventType? eventType, string description, DateTime? eventDate, string timeSlot, string venue, string city, string guestRange, string budgetRange, decimal? actualBudget, decimal? spentAmount, EventStatus? status, string coverImageUrl, string eventImages);
        Task<string> UploadCoverImageAsync(int eventId, string fileName);
    }
}
