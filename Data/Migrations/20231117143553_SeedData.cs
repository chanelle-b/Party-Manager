using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicrosoftAssignment2.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "Id", "Description", "EventDate", "Location" },
                values: new object[] { 5, "Christmas Party", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "123 Party Lane" });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "Id", "Description", "EventDate", "Location" },
                values: new object[] { 6, "New Year's Eve Celebration", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Downtown Club" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Parties",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}