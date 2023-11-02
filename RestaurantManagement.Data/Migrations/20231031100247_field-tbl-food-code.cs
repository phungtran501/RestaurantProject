using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Data.Migrations
{
    public partial class fieldtblfoodcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Food",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Food");
        }
    }
}
