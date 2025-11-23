using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceG02.Presistence.Migrations.StoreIdentityDb
{
    /// <inheritdoc />
    public partial class UpdateAddressmodelAddFirstNameLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Adresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Adresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Adresses");
        }
    }
}
