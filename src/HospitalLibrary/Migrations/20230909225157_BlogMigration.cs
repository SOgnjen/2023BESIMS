using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HospitalLibrary.Migrations
{
    public partial class BlogMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WriterJmbg = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Text", "Title", "WriterJmbg" },
                values: new object[,]
                {
                    { 1, "Regular check-ups are essential for maintaining good health. They help detect health issues early and prevent complications. Make sure to schedule your check-up today!", "The Importance of Regular Check-ups", 987654321 },
                    { 2, "Healthy skin is a reflection of overall well-being. In this blog, we'll share tips for maintaining healthy and radiant skin. Remember to stay hydrated and protect your skin from the sun!", "Tips for Healthy Skin", 22222222 },
                    { 3, "Stress is a common issue in today's fast-paced world. In this blog, we'll delve into the causes of stress and provide effective coping strategies. Remember, it's essential to prioritize your mental health.", "Understanding Stress and Coping Strategies", 33333333 },
                    { 4, "Exercise not only benefits your body but also your brain. Learn how physical activity can boost cognitive function, improve memory, and reduce the risk of neurological conditions. Get moving for a healthier brain!", "The Brain-Boosting Benefits of Exercise", 44444444 },
                    { 5, "Proper nutrition is the cornerstone of good health. This blog explores the importance of a balanced diet, the role of nutrients, and how dietary choices can affect your overall well-being.", "Nutrition and Its Impact on Overall Health", 987654321 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
