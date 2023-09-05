using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HospitalLibrary.Migrations
{
    public partial class HealthInfoMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OwnersJmbg = table.Column<int>(type: "integer", nullable: false),
                    UpperBloodPreassure = table.Column<int>(type: "integer", nullable: false),
                    LowerBloodPreassure = table.Column<int>(type: "integer", nullable: false),
                    SugarLever = table.Column<int>(type: "integer", nullable: false),
                    FatPercentage = table.Column<double>(type: "double precision", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    LastMenstruation = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthInfos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HealthInfos",
                columns: new[] { "Id", "Date", "FatPercentage", "LastMenstruation", "LowerBloodPreassure", "OwnersJmbg", "SugarLever", "UpperBloodPreassure", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 20.5, new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, 1234567890, 90, 120, 70.0 },
                    { 2, new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 22.0, new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85, 987654321, 95, 130, 72.5 },
                    { 3, new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 18.5, new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, 11111111, 88, 140, 68.799999999999997 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthInfos");
        }
    }
}
