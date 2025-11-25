using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wedding_Planner.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReviewImages",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MessageType",
                table: "Messages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "Amount", "CreatedAt", "EventId", "Notes", "PaidAmount", "PaymentMethod", "PaymentStatus", "ServiceDate", "ServiceDescription", "ServiceName", "Status", "TimeSlot", "UpdatedAt", "VendorId" },
                values: new object[] { 1, 150000m, new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Vegetarian options required", 50000m, "Bank Transfer", 2, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Full catering service for 500 guests", "Catering", 2, "Evening", null, 1 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedAt", "IsRead", "Message", "Priority", "ReadAt", "Title", "Type", "UserId" },
                values: new object[] { 1, new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, "Your catering booking has been confirmed for December 15, 2025.", 3, null, "Booking Confirmed", 2, 1 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "EventId", "EventPlannerId", "IsApproved", "Rating", "ReviewImages", "Title", "UserId", "VendorId" },
                values: new object[] { 1, "Great food and service!", new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 5, null, "Excellent Service", 1, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$K2iBIg2badHUu/eKkqZ5..rJxANBelQs7IWZPe/CVQZG5XoQpyoi6");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "City", "CreatedAt", "Email", "FirstName", "IsActive", "IsEmailVerified", "LastName", "PasswordHash", "PhoneNumber", "ProfileImageUrl", "Role", "Title", "UpdatedAt" },
                values: new object[] { 4, "Mumbai", new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "admin@weddingplanner.com", "Admin", true, true, "User", "$2a$11$vI8aWBnW3fID.ZQ4/zo1G.q1lRps.9cGLcZEiGDMVr5yUP1KUOYTa", "9999999999", "/images/users/admin.png", 4, "Mr", null });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "EventId", "IsRead", "MessageType", "Priority", "ReadAt", "ReceiverId", "SenderId", "SentAt", "Subject" },
                values: new object[] { 1, "Hi, I would like to discuss catering options for my wedding.", 1, false, "Inquiry", 2, null, 4, 1, new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Catering Inquiry" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewImages",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MessageType",
                table: "Messages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "hashedpassword123");
        }
    }
}
