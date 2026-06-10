using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPlayAgainToMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlayAgain",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlayAgain",
                table: "Matches");
        }
    }
}
