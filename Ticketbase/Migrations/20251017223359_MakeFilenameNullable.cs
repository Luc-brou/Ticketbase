using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketbase.Migrations
{
    /// <inheritdoc />
    public partial class MakeFilenameNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Concerts_GenreID",
                table: "Concerts");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "Concerts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_GenreID",
                table: "Concerts",
                column: "GenreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Concerts_GenreID",
                table: "Concerts");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "Concerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_GenreID",
                table: "Concerts",
                column: "GenreID",
                unique: true);
        }
    }
}
