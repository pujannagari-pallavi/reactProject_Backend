using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Constants;

namespace Wedding_Planner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IMapper mapper, IBookingService bookingService, IUserService userService)
        {
            _messageService = messageService;
            _mapper = mapper;
            _bookingService = bookingService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

        [HttpGet("sender/{senderId}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetBySender(int senderId)
        {
            var messages = await _messageService.GetMessagesBySenderAsync(senderId);
            return Ok(messages);
        }

        [HttpGet("receiver/{receiverId}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetByReceiver(int receiverId)
        {
            var messages = await _messageService.GetMessagesByReceiverAsync(receiverId);
            return Ok(messages);
        }

        [HttpPost("user-messages")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetByUser([FromBody] UserIdRequestDto request)
        {
            var sent = await _messageService.GetMessagesBySenderAsync(request.UserId);
            var received = await _messageService.GetMessagesByReceiverAsync(request.UserId);
            var allMessages = sent.Concat(received).OrderByDescending(m => m.SentAt);
            return Ok(allMessages);
        }

        [HttpGet("conversation/{userId1}/{userId2}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetConversation(int userId1, int userId2)
        {
            var messages = await _messageService.GetConversationAsync(userId1, userId2);
            return Ok(messages);
        }

        [HttpGet("unread/{userId}")]
        [Authorize(Roles =  Roles.All)]
        public async Task<IActionResult> GetUnread(int userId)
        {
            var messages = await _messageService.GetUnreadMessagesAsync(userId);
            return Ok(messages);
        }

        //Updated Create Endpoint
        [HttpPost]
        [Authorize(Roles = Roles.ClientPlannerVendor)]
        public async Task<IActionResult> Create([FromBody] MessageCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new { success = false, message = ErrorMessages.InvalidData, errors = ModelState });

                var created = await _messageService.CreateMessageFromDtoAsync(dto.SenderId, dto.ReceiverId, dto.Content, dto.Subject, dto.Priority);
                
                if (created == null || created.Id == 0)
                    return Ok(new { success = false, message = ErrorMessages.MessageSaveFailed });
                    
                return Ok(new { success = true, data = created, message = SuccessMessages.MessageSent });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ErrorMessages.MessageCreateFailed, error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}/mark-read")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _messageService.MarkAsReadAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = SuccessMessages.MessageMarkedAsRead });
        }

        [HttpPut("{id}/read")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> MarkRead(int id)
        {
            var result = await _messageService.MarkAsReadAsync(id);
            if (!result)
                return NotFound();
            var message = await _messageService.GetByIdAsync(id);
            return Ok(message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _messageService.DeleteAsync(id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }

        [HttpPost("user-contacts")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetContacts([FromBody] UserIdRequestDto request)
        {
            var contacts = await _messageService.GetUserContactsAsync(request.UserId);
            return Ok(contacts);
        }

        [HttpPost("user-data")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUserMessagesAndContacts([FromBody] UserIdRequestDto request)
        {
            var sent = await _messageService.GetMessagesBySenderAsync(request.UserId);
            var received = await _messageService.GetMessagesByReceiverAsync(request.UserId);
            var allMessages = sent.Concat(received).OrderByDescending(m => m.SentAt).ToList();
            var contacts = await _messageService.GetUserContactsAsync(request.UserId);
            
            return Ok(new { messages = allMessages, contacts });
        }
    }
}
