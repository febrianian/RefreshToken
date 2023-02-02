using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefreshToken.Migrations
{
    public partial class RemoveRolesFromUserClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
