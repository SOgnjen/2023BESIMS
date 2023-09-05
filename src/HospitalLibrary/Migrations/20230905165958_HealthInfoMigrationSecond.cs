using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalLibrary.Migrations
{
    public partial class HealthInfoMigrationSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HealthInfos",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnersJmbg",
                value: 1234567890);

            migrationBuilder.UpdateData(
                table: "HealthInfos",
                keyColumn: "Id",
                keyValue: 3,
                column: "OwnersJmbg",
                value: 1234567890);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HealthInfos",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnersJmbg",
                value: 987654321);

            migrationBuilder.UpdateData(
                table: "HealthInfos",
                keyColumn: "Id",
                keyValue: 3,
                column: "OwnersJmbg",
                value: 11111111);
        }
    }
}
