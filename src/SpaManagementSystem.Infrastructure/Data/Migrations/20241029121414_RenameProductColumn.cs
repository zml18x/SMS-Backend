using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3da593c4-61db-4e9c-a15d-c16422bb0694"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8ce5601f-a4bd-4239-ae4b-5bf9a6d7cacd"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f25fb3a4-2521-4b8b-9933-d23a613b51ec"));

            migrationBuilder.DropColumn(
                name: "CreatedByEmployeeId",
                schema: "SMS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedByEmployeeId",
                schema: "SMS",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "MinimumStockLevel",
                schema: "SMS",
                table: "Products",
                newName: "MinimumStockQuantity");

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("15ff8107-9212-4973-86ba-5fea2d477017"), "8790caa9-d01a-4cae-a461-d030480f3253", "Employee", "EMPLOYEE" },
                    { new Guid("87728b5f-4a62-4882-b4ef-bac69abae097"), "ed0d64f7-41bf-49af-a672-c6f8ca26edfc", "Admin", "ADMIN" },
                    { new Guid("d5a50067-a9a0-4174-aa1e-e04dad221537"), "fca18eed-3649-4449-899a-c4dab067e94b", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("15ff8107-9212-4973-86ba-5fea2d477017"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("87728b5f-4a62-4882-b4ef-bac69abae097"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d5a50067-a9a0-4174-aa1e-e04dad221537"));

            migrationBuilder.RenameColumn(
                name: "MinimumStockQuantity",
                schema: "SMS",
                table: "Products",
                newName: "MinimumStockLevel");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByEmployeeId",
                schema: "SMS",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByEmployeeId",
                schema: "SMS",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3da593c4-61db-4e9c-a15d-c16422bb0694"), "eee6e429-8b2f-4967-afc1-d12d1ad42572", "Admin", "ADMIN" },
                    { new Guid("8ce5601f-a4bd-4239-ae4b-5bf9a6d7cacd"), "20c5d0a2-8304-4bf0-802c-0933a57f42a1", "Manager", "MANAGER" },
                    { new Guid("f25fb3a4-2521-4b8b-9933-d23a613b51ec"), "4f56b52c-54dc-4fa3-9da1-5498b511d0dc", "Employee", "EMPLOYEE" }
                });
        }
    }
}
