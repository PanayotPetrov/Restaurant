namespace Restaurant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangedTheTypeOfForeignKeyPropertyAddressNameInOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressName",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Name",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "AddressName",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "ItemTotalPrice",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Addresses_Name",
                table: "Addresses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Name",
                table: "Addresses",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressName",
                table: "Orders",
                column: "AddressName",
                principalTable: "Addresses",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressName",
                table: "Orders");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Addresses_Name",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Name",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ItemTotalPrice",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "AddressName",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Name",
                table: "Addresses",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressName",
                table: "Orders",
                column: "AddressName",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
