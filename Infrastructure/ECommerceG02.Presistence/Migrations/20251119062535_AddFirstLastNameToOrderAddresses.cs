using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceG02.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstLastNameToOrderAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "OrderAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "OrderAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "OrderAddresses");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "OrderAddresses");
        }
    }
}
