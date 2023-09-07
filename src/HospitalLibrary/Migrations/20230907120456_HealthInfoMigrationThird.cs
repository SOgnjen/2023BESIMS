using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalLibrary.Migrations
{
    public partial class HealthInfoMigrationThird : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SugarLever",
                table: "HealthInfos",
                newName: "SugarLevel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SugarLevel",
                table: "HealthInfos",
                newName: "SugarLever");
        }
    }
}
