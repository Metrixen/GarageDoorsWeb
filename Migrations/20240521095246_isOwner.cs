using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageDoorsWeb.Migrations
{
    public partial class isOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isOwner",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOwner",
                table: "Users");
        }
    }
}
