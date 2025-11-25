namespace Wedding_Planner.Domain.Constants
{
    // Numeric role mapping (for DB)
    public static class UserRoles
    {
        public const int Client = 1;
        public const int EventPlanner = 2;
        public const int Vendor = 3;
        public const int Admin = 4;
    }

    // String role names (used in [Authorize])
    public static class Roles
    {
        public const string Client = "Client";
        public const string EventPlanner = "EventPlanner";
        public const string Vendor = "Vendor";
        public const string Admin = "Admin";

        // Combined roles
        public const string ClientEventPlanner = Client + "," + EventPlanner;
        public const string ClientEventPlannerAdmin = Client + "," + EventPlanner + "," + Admin;
        public const string VendorAdmin = Vendor + "," + Admin;
        public const string ClientPlannerVendor = Client + "," + EventPlanner + "," + Vendor;
        public const string All = Client + "," + EventPlanner + "," + Vendor + "," + Admin;
    }

    public static class ErrorMessages
    {
        public const string ValidationFailed = "Validation failed";
        public const string RegistrationError = "An error occurred during registration";
        public const string LoginError = "An error occurred during login";
        public const string NoFileUploaded = "No file uploaded";
        public const string NotFound = "The requested resource was not found.";
        public const string ContactFormFailed = "Failed to submit contact form";
        public const string ContactFormProcessFailed = "Failed to process contact form";
        public const string MessageCreateFailed = "Failed to create message";
        public const string InvalidData = "Invalid data provided";
        public const string MessageSaveFailed = "Failed to save message to database";
        public const string NoInnerException = "No inner exception";
    }

    public static class SuccessMessages
    {
        public const string DeletedSuccessfully = "Deleted successfully";
        public const string MarkedAsRead = "marked as read";
        public const string RegistrationSuccessful = "Registration successful";
        public const string LoginSuccessful = "Login successful";
        public const string ContactFormSubmitted = "Contact form submitted successfully";
        public const string MessageSent = "Message sent successfully";
        public const string MessageMarkedAsRead = "Message marked as read";
        public const string VendorVerified = "Vendor verified successfully";
        public const string VendorRejected = "Vendor verification rejected";
        public const string EventPlannerVerified = "Event planner verified successfully";
        public const string EventPlannerRejected = "Event planner verification rejected";
        public const string ReviewApproved = "Review approved";
        public const string ImageDeleted = "Image deleted";
        public const string AllNotificationsRead = "All notifications marked as read";
       
    }

    public static class DefaultValues
    {
        public const string DefaultVendorLogo = "/images/vendors/default-logo.png";
        public const string EmptyJsonArray = "[]";
        public const string DefaultEventCover = "/uploads/events/default-event.jpg";
        public const string DefaultVendorDescription = "Professional service provider";
    }

    public static class JwtConfig
    {
        public const string Issuer = "Jwt:Issuer";
        public const string Audience = "Jwt:Audience";
        public const string Key = "Jwt:Key";
        public const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    }

    public static class CorsPolicy
    {
        public const string AllowReactApp = "AllowReactApp";

        public static readonly string[] AllowedOrigins =
        {
            "http://localhost:5173",
            "http://localhost:5174",
            "http://localhost:3000"
        };
    }

    public static class ConnectionStrings
    {
        public const string DefaultConnection = "DefaultConnection";
    }

    public static class AuthSchemes
    {
        public const string Bearer = "Bearer";
    }

    public static class SwaggerConfig
    {
        public const string SecurityDefinitionName = "Bearer";
        public const string AuthorizationHeader = "Authorization";
        public const string BearerFormat = "JWT";
        public const string Description = "Enter JWT token like: Bearer {your token}";
    }

    public static class NotificationTexts
    {
        public const string NewBookingTitle = "New Booking Received";

        public static string NewBookingMessage(string service, DateTime date) =>
            $"You have a new booking for {service} on {date:MMM dd, yyyy}";

        public static string StatusTitle(string status) => $"Booking {status}";

        public static string StatusMessage(string service, string status) =>
            $"Your booking for {service} has been {status}";
    }

    public static class BookingErrors
    {
        public static string NotFound(int id) => $"Booking with ID {id} not found.";
        public const string NotificationFailed = "Failed to send notification: ";
    }

    public static class BookingRoutes
    {
        public const string GetById = "{id}";
        public const string Event = "event/{eventId}";
        public const string VendorBookings = "vendor-bookings";
        public const string Status = "status/{status}";
        public const string PendingPayments = "pending-payments";
        public const string TotalAmount = "total-amount/{eventId}";
        public const string UserBookings = "user/{userId}";
    }

    public static class DashboardRoutes
    {
        public const string Base = "api/dashboard";
        public const string Admin = "admin";
        public const string Client = "client-dashboard";
        public const string Vendor = "vendor-dashboard";
        public const string Stats = "stats";
        public const string UserStats = "user-stats";
    }
    public static class FilePaths
    {
        public const string EventUploadFolder = "wwwroot/uploads/events";
        public const string EventUploadUrl = "/uploads/events/";
    }
    public static class EventTaskRoutes
    {
        public const string GetById = "{id}";
        public const string GetByEvent = "event/{eventId}";
        public const string GetByStatus = "status/{status}";
        public const string GetPending = "pending";
        public const string GetByPriority = "priority/{priority}";
        public const string GetOverdue = "overdue";
        public const string DueSoon = "due-soon";
    }

    public static class EventTaskMessages
    {
        public const string Completed = "Task marked as completed";
    }
    public static class ApiRoutes
    {
        public const string Base = "api/[controller]";

        public const string GetById = "{id}";
        public const string Sender = "sender/{senderId}";
        public const string Receiver = "receiver/{receiverId}";
        public const string UserMessages = "user-messages";
        public const string Conversation = "conversation/{userId1}/{userId2}";
        public const string Unread = "unread/{userId}";
        public const string MarkRead = "{id}/mark-read";
        public const string Read = "{id}/read";
        public const string UserContacts = "user-contacts";
    }
    public static class AppRoles
    {
        public const string Client = "Client";
        public const string EventPlanner = "EventPlanner";
        public const string Vendor = "Vendor";
        public const string Admin = "Admin";

        public const string AllCommonRoles = Client + "," + EventPlanner + "," + Vendor + "," + Admin;
        public const string ClientPlannerVendor = Client + "," + EventPlanner + "," + Vendor;
    }

}
