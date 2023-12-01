#nullable disable
namespace Restaurant.Data.Migrations
{

using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
public partial class AddingSecondayCultureSupportToCategoryAndUserCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondaryName",
                table: "UserMessageCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryAdjective",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryName",
                table: "UserMessageCategories");

            migrationBuilder.DropColumn(
                name: "SecondaryAdjective",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SecondaryName",
                table: "Categories");
        }
    }
}
