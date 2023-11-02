using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Data.Migrations
{
    public partial class addfieldcreatedontblcomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Comment");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Comment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
