using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefreshToken.Migrations
{
    public partial class modifyRolesDetailClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolesDetail_Roles_RoleId",
                table: "RolesDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RolesDetail_User_UserId",
                table: "RolesDetail");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RolesDetail",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RolesDetail",
                newName: "IdRoles");

            migrationBuilder.RenameIndex(
                name: "IX_RolesDetail_UserId",
                table: "RolesDetail",
                newName: "IX_RolesDetail_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_RolesDetail_RoleId",
                table: "RolesDetail",
                newName: "IX_RolesDetail_IdRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_RolesDetail_Roles_IdRoles",
                table: "RolesDetail",
                column: "IdRoles",
                principalTable: "Roles",
                principalColumn: "IdRoles",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolesDetail_User_IdUser",
                table: "RolesDetail",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolesDetail_Roles_IdRoles",
                table: "RolesDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RolesDetail_User_IdUser",
                table: "RolesDetail");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "RolesDetail",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRoles",
                table: "RolesDetail",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolesDetail_IdUser",
                table: "RolesDetail",
                newName: "IX_RolesDetail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RolesDetail_IdRoles",
                table: "RolesDetail",
                newName: "IX_RolesDetail_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolesDetail_Roles_RoleId",
                table: "RolesDetail",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "IdRoles",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolesDetail_User_UserId",
                table: "RolesDetail",
                column: "UserId",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
