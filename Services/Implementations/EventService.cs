using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class EventService : GenericService<Event>, IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly INotificationRepository _notificationRepository;

        public EventService(IEventRepository eventRepository, INotificationRepository notificationRepository) : base(eventRepository)
        {
            _eventRepository = eventRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
        {
            await UpdatePastEventsStatusAsync();
            return await _eventRepository.GetEventsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status)
        {
            return await _eventRepository.GetEventsByStatusAsync(status);
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            return await _eventRepository.GetUpcomingEventsAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _eventRepository.GetEventsByDateRangeAsync(startDate, endDate);
        }

        public async Task<Event> GetEventWithDetailsAsync(int eventId)
        {
            return await _eventRepository.GetEventWithDetailsAsync(eventId);
        }

        public override async Task<Event> CreateAsync(Event eventEntity)
        {
            eventEntity.CreatedAt = DateTime.UtcNow;
            eventEntity.Status = EventStatus.Planning;
            
            // Set defaults for all NOT NULL string fields
            if (string.IsNullOrEmpty(eventEntity.CoverImageUrl))
                eventEntity.CoverImageUrl = "/uploads/events/default-event.jpg";
            
            if (string.IsNullOrEmpty(eventEntity.EventImages))
                eventEntity.EventImages = "[]";
            
            if (string.IsNullOrEmpty(eventEntity.TimeSlot))
                eventEntity.TimeSlot = "Not Specified";
            
            if (string.IsNullOrEmpty(eventEntity.Description))
                eventEntity.Description = "";
            
            if (string.IsNullOrEmpty(eventEntity.Venue))
                eventEntity.Venue = "";
            
            if (string.IsNullOrEmpty(eventEntity.City))
                eventEntity.City = "";
            
            if (string.IsNullOrEmpty(eventEntity.GuestRange))
                eventEntity.GuestRange = "Not Specified";
            
            if (string.IsNullOrEmpty(eventEntity.BudgetRange))
                eventEntity.BudgetRange = "Not Specified";
            
            return await base.CreateAsync(eventEntity);
        }

        public override async Task<Event> UpdateAsync(Event eventEntity)
        {
            eventEntity.UpdatedAt = DateTime.UtcNow;
            return await base.UpdateAsync(eventEntity);
        }

        public async Task<Event> PartialUpdateAsync(int id, string title, EventType? eventType, string description, DateTime? eventDate, string timeSlot, string venue, string city, string guestRange, string budgetRange, decimal? actualBudget, decimal? spentAmount, EventStatus? status, string coverImageUrl, string eventImages)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent == null) return null;

            if (title != null) existingEvent.Title = title;
            if (eventType.HasValue) existingEvent.EventType = eventType.Value;
            if (description != null) existingEvent.Description = description;
            if (eventDate.HasValue) existingEvent.EventDate = eventDate.Value;
            if (timeSlot != null) existingEvent.TimeSlot = timeSlot;
            if (venue != null) existingEvent.Venue = venue;
            if (city != null) existingEvent.City = city;
            if (guestRange != null) existingEvent.GuestRange = guestRange;
            if (budgetRange != null) existingEvent.BudgetRange = budgetRange;
            if (actualBudget.HasValue) existingEvent.ActualBudget = actualBudget.Value;
            if (spentAmount.HasValue) existingEvent.SpentAmount = spentAmount.Value;
            if (status.HasValue) existingEvent.Status = status.Value;
            if (coverImageUrl != null) existingEvent.CoverImageUrl = coverImageUrl;
            if (eventImages != null) existingEvent.EventImages = eventImages;

            return await UpdateAsync(existingEvent);
        }

        public async Task<string> UploadCoverImageAsync(int eventId, string fileName)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null) return null;

            eventEntity.CoverImageUrl = "/uploads/events/" + fileName;
            await UpdateAsync(eventEntity);
            return eventEntity.CoverImageUrl;
        }

        public async Task UpdatePastEventsStatusAsync()
        {
            var allEvents = await _eventRepository.GetAllAsync();
            var today = DateTime.UtcNow.Date;
            
            foreach (var evt in allEvents)
            {
                if (evt.EventDate.Date < today && evt.Status != EventStatus.Completed && evt.Status != EventStatus.Cancelled)
                {
                    evt.Status = EventStatus.Completed;
                    evt.UpdatedAt = DateTime.UtcNow;
                    await _eventRepository.UpdateAsync(evt);
                    
                    // Create notification for review request
                    await _notificationRepository.AddAsync(new Notification
                    {
                        UserId = evt.UserId,
                        Title = "Event Completed - Share Your Experience!",
                        Message = $"Your event '{evt.Title}' has been completed successfully! Please take a moment to rate and review the vendors who made your event special.",
                        Type = NotificationType.Event,
                        Priority = NotificationPriority.High,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
