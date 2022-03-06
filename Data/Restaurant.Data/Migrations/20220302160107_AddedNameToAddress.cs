namespace Restaurant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedNameToAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_ApplicationUserId",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ApplicationUserId",
                table: "Addresses",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_ApplicationUserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ApplicationUserId",
                table: "Addresses",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");
        }
    }
}
