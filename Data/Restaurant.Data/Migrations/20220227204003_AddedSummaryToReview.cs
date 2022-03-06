namespace Restaurant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedSummaryToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Reviews",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Reviews");
        }
    }
}
