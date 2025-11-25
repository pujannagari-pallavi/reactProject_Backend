using AutoMapper;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<ClientRegisterDto, User>();
            CreateMap<EventPlannerRegisterDto, User>();
            CreateMap<VendorRegisterDto, User>();
            CreateMap<VendorRegisterDto, Vendor>();
            CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Vendor mappings
            CreateMap<Vendor, VendorUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Event mappings
            CreateMap<EventCreateDto, Event>();
            CreateMap<Event, EventCreateDto>();
            CreateMap<EventUpdateDto, Event>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Booking mappings
            CreateMap<BookingCreateDto, Booking>();
            CreateMap<Booking, BookingCreateDto>();
            CreateMap<BookingUpdateDto, Booking>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // EventTask mappings
            CreateMap<EventTaskCreateDto, EventTask>();
            CreateMap<EventTask, EventTaskCreateDto>();
            CreateMap<EventTaskUpdateDto, EventTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Message mappings
            CreateMap<MessageCreateDto, Message>();
            CreateMap<Message, MessageCreateDto>();

            // Notification mappings
            CreateMap<NotificationCreateDto, Notification>();
            CreateMap<Notification, NotificationCreateDto>();

            // Review mappings
            CreateMap<ReviewCreateDto, Review>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => $"{src.Rating} Star Review"));

            // Dashboard mappings
            CreateMap<ClientDashboardDto, ClientDashboardDto>();
        }
    }
}