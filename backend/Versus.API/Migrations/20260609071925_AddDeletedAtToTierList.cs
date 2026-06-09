using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedAtToTierList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TierList",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TierList");
        }
    }
}
