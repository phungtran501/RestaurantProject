using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Data.Migrations
{
    public partial class updatefoodremovefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Food");

            migrationBuilder.AlterColumn<int>(
                name: "Available",
                table: "Food",
                type: "int",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Available",
                table: "Food",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Food",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Food",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
