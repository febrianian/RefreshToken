using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefreshToken.Migrations
{
    public partial class AddRolesDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolesDetail",
                columns: table => new
                {
                    IdRolesDetail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserModified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesDetail", x => x.IdRolesDetail);
                    table.ForeignKey(
                        name: "FK_RolesDetail_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "IdRoles",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesDetail_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesDetail_RoleId",
                table: "RolesDetail",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesDetail_UserId",
                table: "RolesDetail",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesDetail");
        }
    }
}
