using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HospitalLibrary.Migrations
{
    public partial class AppointmentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorJmbg = table.Column<int>(type: "integer", nullable: false),
                    PatientJmbg = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "Date", "DoctorJmbg", "PatientJmbg" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 987654321, 1234567890 },
                    { 2, new DateTime(2023, 10, 18, 14, 30, 0, 0, DateTimeKind.Unspecified), 987654321, 1234567890 },
                    { 3, new DateTime(2023, 10, 20, 9, 15, 0, 0, DateTimeKind.Unspecified), 987654321, 1234567890 },
                    { 4, new DateTime(2023, 10, 22, 11, 0, 0, 0, DateTimeKind.Unspecified), 22222222, 1234567890 },
                    { 5, new DateTime(2023, 10, 25, 16, 45, 0, 0, DateTimeKind.Unspecified), 22222222, 0 },
                    { 6, new DateTime(2023, 10, 28, 8, 30, 0, 0, DateTimeKind.Unspecified), 44444444, 0 },
                    { 7, new DateTime(2023, 10, 30, 13, 15, 0, 0, DateTimeKind.Unspecified), 44444444, 0 },
                    { 8, new DateTime(2023, 11, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 33333333, 0 },
                    { 9, new DateTime(2023, 11, 5, 15, 30, 0, 0, DateTimeKind.Unspecified), 33333333, 0 },
                    { 10, new DateTime(2023, 11, 8, 10, 45, 0, 0, DateTimeKind.Unspecified), 22222222, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Emails", "FirstName", "Gender", "Jmbg", "LastName", "Password", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 4, "321 Oak St", "sarah.brown@example.com", "Sarah", 0, 22222222, "Brown", "password", "555-4321", 3 },
                    { 5, "567 Pine St", "michael.clark@example.com", "Michael", 1, 44444444, "Clark", "password", "555-8765", 4 },
                    { 6, "789 Cedar St", "emily.wilson@example.com", "Emily", 0, 33333333, "Wilson", "password", "555-9876", 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
