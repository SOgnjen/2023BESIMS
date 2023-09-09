using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HospitalLibrary.Migrations
{
    public partial class HealthReviewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PatientJmbg = table.Column<int>(type: "integer", nullable: false),
                    Diagnosis = table.Column<string>(type: "text", nullable: false),
                    Cure = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthReviews", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HealthReviews",
                columns: new[] { "Id", "Cure", "Date", "Diagnosis", "PatientJmbg" },
                values: new object[,]
                {
                    { 1, "Rest and fluids", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Common Cold", 1234567890 },
                    { 2, "Antihistamines prescribed", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Allergic Reaction", 1234567890 },
                    { 3, "RICE protocol (Rest, Ice, Compression, Elevation)", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprained Ankle", 1234567890 },
                    { 4, "Pain relievers and rest advised", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Migraine Headache", 1234567890 },
                    { 5, "Dietary changes recommended", new DateTime(2023, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stomachache", 1234567890 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guidance",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthReviews");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guidance",
                value: 0);
        }
    }
}
