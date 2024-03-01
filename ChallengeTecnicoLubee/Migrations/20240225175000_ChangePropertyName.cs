using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeTecnicoLubee.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearsOfAge",
                table: "Publications",
                newName: "Antiquity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Antiquity",
                table: "Publications",
                newName: "YearsOfAge");
        }
    }
}
