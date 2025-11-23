using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceG02.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class StoreDbContext02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "OrderAddresses",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "OrderAddresses",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "OrderAddresses",
                newName: "AddressLine2");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "OrderAddresses",
                newName: "AddressLine1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrderAddresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrderAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "OrderAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrderAddresses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrderAddresses");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "OrderAddresses");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "OrderAddresses",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "OrderAddresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "OrderAddresses",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                table: "OrderAddresses",
                newName: "FirstName");
        }
    }
}
