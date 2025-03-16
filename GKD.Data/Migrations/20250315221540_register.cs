using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GKD.Data.Migrations
{
    /// <inheritdoc />
    public partial class register : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_Role_RolesId",
                table: "PermissionRole");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Role_RolesId",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "_Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Roles",
                table: "_Roles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX__Users_Email",
                table: "_Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole__Roles_RolesId",
                table: "PermissionRole",
                column: "RolesId",
                principalTable: "_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser__Roles_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole__Roles_RolesId",
                table: "PermissionRole");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser__Roles_RolesId",
                table: "RoleUser");

            migrationBuilder.DropIndex(
                name: "IX__Users_Email",
                table: "_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Roles",
                table: "_Roles");

            migrationBuilder.RenameTable(
                name: "_Roles",
                newName: "Role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_Role_RolesId",
                table: "PermissionRole",
                column: "RolesId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Role_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
