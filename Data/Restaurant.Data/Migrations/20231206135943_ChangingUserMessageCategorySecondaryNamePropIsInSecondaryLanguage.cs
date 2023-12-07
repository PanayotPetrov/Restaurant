using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangingUserMessageCategorySecondaryNamePropIsInSecondaryLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryName",
                table: "UserMessageCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsInSecondaryLanguage",
                table: "UserMessageCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInSecondaryLanguage",
                table: "UserMessageCategories");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryName",
                table: "UserMessageCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
