using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GKD.Data.Migrations
{
    /// <inheritdoc />
    public partial class GKSdb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Files__Users_UserId",
                table: "_Files");

            migrationBuilder.DropIndex(
                name: "IX__Files_UserId",
                table: "_Files");

            migrationBuilder.DropColumn(
                name: "FilesId",
                table: "_Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "_Files");

            migrationBuilder.CreateIndex(
                name: "IX__Files_OwnerId",
                table: "_Files",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK__Files__Users_OwnerId",
                table: "_Files",
                column: "OwnerId",
                principalTable: "_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Files__Users_OwnerId",
                table: "_Files");

            migrationBuilder.DropIndex(
                name: "IX__Files_OwnerId",
                table: "_Files");

            migrationBuilder.AddColumn<List<int>>(
                name: "FilesId",
                table: "_Users",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "_Files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX__Files_UserId",
                table: "_Files",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Files__Users_UserId",
                table: "_Files",
                column: "UserId",
                principalTable: "_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
