using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GKD.Data.Migrations
{
    /// <inheritdoc />
    public partial class fileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "_Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "_Files");
        }
    }
}
