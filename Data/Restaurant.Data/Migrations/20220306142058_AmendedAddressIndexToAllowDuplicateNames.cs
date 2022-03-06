namespace Restaurant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AmendedAddressIndexToAllowDuplicateNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_Name",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Name",
                table: "Addresses",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_Name",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Name",
                table: "Addresses",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
