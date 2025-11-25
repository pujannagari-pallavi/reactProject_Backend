namespace Wedding_Planner.Application.DTOs
{
    public class IdRequestDto
    {
        public int Id { get; set; }
    }

    public class DeleteRequestDto
    {
        public int Id { get; set; }
    }

    public class UserIdRequestDto
    {
        public int UserId { get; set; }
    }

    public class VendorIdRequestDto
    {
        public int VendorId { get; set; }
    }

    public class EventIdRequestDto
    {
        public int EventId { get; set; }
    }

    public class ConversationRequestDto
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
    }
}
