using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Domain.Entities;



namespace Wedding_Planner.Data.Data
{
    public class WeddingPlannerDbContext: DbContext
    {
        public WeddingPlannerDbContext(DbContextOptions<WeddingPlannerDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventTask> EventTasks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------- USER CONFIGURATION -------------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Title).HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Role).HasConversion<int>();
            });

            // ------------------- EVENT CONFIGURATION -------------------
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.EventType).HasConversion<string>();
                entity.Property(e => e.TimeSlot).HasMaxLength(50);
                entity.Property(e => e.Venue).HasMaxLength(200);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.GuestRange).HasMaxLength(20);
                entity.Property(e => e.BudgetRange).HasMaxLength(20);
                entity.Property(e => e.ActualBudget).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SpentAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
                entity.Property(e => e.EventImages).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Status).HasConversion<int>();

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Events)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ------------------- VENDOR CONFIGURATION -------------------
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BusinessName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.ContactPerson).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Services).HasColumnType("nvarchar(max)");
                entity.Property(e => e.PriceRange).HasMaxLength(50);
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.LogoUrl).HasMaxLength(500).IsRequired(false);
                entity.Property(e => e.GalleryImages).HasColumnType("nvarchar(max)").IsRequired(false);
            });

            // ------------------- BOOKING CONFIGURATION -------------------
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServiceName).HasMaxLength(200);
                entity.Property(e => e.ServiceDescription).HasMaxLength(1000);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaidAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TimeSlot).HasMaxLength(50);
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.PaymentStatus).HasConversion<int>();

                entity.HasOne(b => b.Event)
                      .WithMany(e => e.Bookings)
                      .HasForeignKey(b => b.EventId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Vendor)
                      .WithMany(v => v.Bookings)
                      .HasForeignKey(b => b.VendorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ------------------- EVENT TASK CONFIGURATION -------------------
            modelBuilder.Entity<EventTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.AssignedTo).HasMaxLength(50);
                entity.Property(e => e.Priority).HasConversion<int>();
                entity.Property(e => e.TaskStatus).HasConversion<int>();

                entity.HasOne(t => t.Event)
                      .WithMany(e => e.Tasks)
                      .HasForeignKey(t => t.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ------------------- MESSAGE CONFIGURATION -------------------
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Subject).HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(e => e.MessageType).HasMaxLength(50);
                entity.Property(e => e.Priority).HasConversion<int>();

                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Event)
                      .WithMany(e => e.Messages)
                      .HasForeignKey(m => m.EventId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ------------------- NOTIFICATION CONFIGURATION -------------------
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(e => e.Type).HasConversion<int>();
                entity.Property(e => e.Priority).HasConversion<int>();

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ------------------- REVIEW CONFIGURATION -------------------
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Comment).HasMaxLength(2000);
                entity.Property(e => e.ReviewImages).HasColumnType("nvarchar(max)");

                entity.HasOne(r => r.User)
                      .WithMany()
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Vendor)
                      .WithMany()
                      .HasForeignKey(r => r.VendorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Event)
                      .WithMany()
                      .HasForeignKey(r => r.EventId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ------------------- SEED DATA -------------------
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "client1@example.com",
                    PasswordHash = "$2a$11$K2iBIg2badHUu/eKkqZ5..rJxANBelQs7IWZPe/CVQZG5XoQpyoi6", // password123
                    Title = "Mr",
                    FirstName = "Arjun",
                    LastName = "Mehta",
                    PhoneNumber = "9876543210",
                    Role = UserRole.Client,
                    ProfileImageUrl = "/images/users/default.png",
                    City = "Bangalore",
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsEmailVerified = true
                },
                new User
                {
                    Id = 4,
                    Email = "admin@weddingplanner.com",
                    PasswordHash = "$2a$11$vI8aWBnW3fID.ZQ4/zo1G.q1lRps.9cGLcZEiGDMVr5yUP1KUOYTa", // admin123
                    Title = "Mr",
                    FirstName = "Admin",
                    LastName = "User",
                    PhoneNumber = "9999999999",
                    Role = UserRole.Admin,
                    ProfileImageUrl = "/images/users/admin.png",
                    City = "Mumbai",
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsEmailVerified = true
                }
             );
            modelBuilder.Entity<Event>().HasData(
            new Event
            {
                Id = 1,
                Title = "Royal Wedding",
                Description = "A luxurious royal-themed wedding event with 500+ guests.",
                EventDate = new DateTime(2025, 12, 15),
                TimeSlot = "Evening",
                Venue = "Bangalore Palace Grounds",
                City = "Bangalore",
                GuestRange = "500+",
                BudgetRange = "5L+",
                ActualBudget = 800000,
                SpentAmount = 200000,
                CoverImageUrl = "images/royalwedding.jpg",
                EventImages = "[]",
                CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                UserId = 1
            }
        );
            // Seed Vendors
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor
                {
                    Id = 1,
                    BusinessName = "Royal Caterers",
                    Category = "Catering",
                    ContactPerson = "Rajesh Kumar",
                    Email = "info@royalcaterers.com",
                    PhoneNumber = "9876543210",
                    City = "Mumbai",
                    Address = "Plot No. 12, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051",
                    Description = "Premium catering services specializing in traditional and modern cuisines for weddings and events.",
                    Services = "[\"Buffet Catering\", \"Live Counters\", \"Beverage Service\", \"Wedding Menu Planning\"]",
                    PriceRange = "Premium",
                    Rating = 4.5m,
                    ReviewCount = 0,
                    LogoUrl = "https://example.com/images/vendors/royalcaterers-logo.jpg",
                    GalleryImages = "[\"catering1.jpg\", \"catering2.jpg\", \"catering3.jpg\"]",
                    IsActive = true,
                    IsVerified = true,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = null
                },
                new Vendor
                {
                    Id = 2,
                    BusinessName = "Capture Moments",
                    Category = "Photography",
                    ContactPerson = "Priya Sharma",
                    Email = "contact@capturemoments.com",
                    PhoneNumber = "9876543211",
                    City = "Delhi",
                    Address = "D-45, South Extension Part 1, New Delhi, Delhi 110049",
                    Description = "Professional wedding and event photography studio capturing timeless moments with artistic precision.",
                    Services = "[\"Wedding Photography\", \"Candid Photography\", \"Cinematic Videography\", \"Pre-Wedding Shoots\"]",
                    PriceRange = "Luxury",
                    Rating = 4.8m,
                    ReviewCount = 0,
                    LogoUrl = "https://example.com/images/vendors/capturemoments-logo.jpg",
                    GalleryImages = "[\"photo1.jpg\", \"photo2.jpg\", \"photo3.jpg\"]",
                    IsActive = true,
                    IsVerified = true,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = null
                },
                new Vendor
                {
                    Id = 3,
                    BusinessName = "Floral Dreams",
                    Category = "Decoration",
                    ContactPerson = "Amit Patel",
                    Email = "info@floraldreams.com",
                    PhoneNumber = "9876543212",
                    City = "Bangalore",
                    Address = "No. 21, MG Road, near Trinity Metro Station, Bengaluru, Karnataka 560001",
                    Description = "Creative floral decorators specializing in elegant wedding stage designs, mandap setups, and venue decorations.",
                    Services = "[\"Stage Decoration\", \"Mandap Setup\", \"Venue Entrance Decor\", \"Floral Arrangements\"]",
                    PriceRange = "Budget",
                    Rating = 4.2m,
                    ReviewCount = 0, // default starting value
                    LogoUrl = "https://example.com/images/vendors/floraldreams-logo.jpg", // optional but useful
                    GalleryImages = "[\"floral1.jpg\", \"floral2.jpg\", \"floral3.jpg\"]", // JSON array
                    IsActive = true,
                    IsVerified = true,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = null
                },
                new Vendor
                {
                    Id = 4,
                    BusinessName = "Grand Palace Venues",
                    Category = "Venue",
                    ContactPerson = "Suresh Reddy",
                    Email = "info@grandpalace.com",
                    PhoneNumber = "9876543213",
                    City = "Bangalore",
                    Address = "Palace Road, Vasanth Nagar, Bengaluru, Karnataka 560052",
                    Description = "Luxury wedding venues and banquet halls with world-class facilities for memorable celebrations.",
                    Services = "[\"Wedding Hall\", \"Banquet Hall\", \"Garden Venue\", \"Conference Hall\"]",
                    PriceRange = "Luxury",
                    Rating = 4.6m,
                    ReviewCount = 0,
                    LogoUrl = "https://example.com/images/vendors/grandpalace-logo.jpg",
                    GalleryImages = "[\"venue1.jpg\", \"venue2.jpg\", \"venue3.jpg\"]",
                    IsActive = true,
                    IsVerified = true,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = null
                }
            );

            // Seed Sample Tasks
            modelBuilder.Entity<EventTask>().HasData(
                new EventTask
                {
                    Id = 1,
                    EventId = 1,
                    Title = "Book Venue",
                    Description = "Confirm venue booking and advance payment",
                    DueDate = new DateTime(2025, 11, 15, 0, 0, 0, DateTimeKind.Utc),
                    Priority = TaskPriority.High,
                    TaskStatus = TaskStatuses.Pending,
                    AssignedTo = "Client",
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc)
                },
                new EventTask
                {
                    Id = 2,
                    EventId = 1,
                    Title = "Finalize Menu",
                    Description = "Select menu items with caterer",
                    DueDate = new DateTime(2025, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                    Priority = TaskPriority.Medium,
                    TaskStatus = TaskStatuses.Pending,
                    AssignedTo = "Planner",
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    ServiceName = "Catering",
                    ServiceDescription = "Full catering service for 500 guests",
                    Amount = 150000,
                    PaidAmount = 50000,
                    ServiceDate = new DateTime(2025, 12, 15),
                    TimeSlot = "Evening",
                    PaymentMethod = "Bank Transfer",
                    Status = BookingStatus.Confirmed,
                    PaymentStatus = PaymentStatus.Partial,
                    Notes = "Vegetarian options required",
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    EventId = 1,
                    VendorId = 1
                }
            );

            // Seed Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    Rating = 5,
                    Title = "Excellent Service",
                    Comment = "Great food and service!",
                    IsApproved = true,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    UserId = 1,
                    VendorId = 1
                }
            );

            // Seed Messages
            modelBuilder.Entity<Message>().HasData(
                new Message
                {
                    Id = 1,
                    Subject = "Catering Inquiry",
                    Content = "Hi, I would like to discuss catering options for my wedding.",
                    MessageType = "Inquiry",
                    Priority = MessagePriority.Medium,
                    IsRead = false,
                    SentAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    SenderId = 1,
                    ReceiverId = 4,
                    EventId = 1
                }
            );

            // Seed Notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification
                {
                    Id = 1,
                    Title = "Booking Confirmed",
                    Message = "Your catering booking has been confirmed for December 15, 2025.",
                    Type = NotificationType.Booking,
                    Priority = NotificationPriority.High,
                    IsRead = false,
                    CreatedAt = new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    UserId = 1
                }
            );
        }

    }
}
