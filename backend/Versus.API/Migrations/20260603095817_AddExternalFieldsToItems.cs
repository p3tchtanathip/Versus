using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.API.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalFieldsToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalSource",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ExternalSource",
                table: "Items");
        }
    }
}
