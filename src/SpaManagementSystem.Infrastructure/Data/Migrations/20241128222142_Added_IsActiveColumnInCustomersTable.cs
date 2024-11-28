using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_IsActiveColumnInCustomersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("80d977c5-10d6-4b9b-b3f4-ff47e5759519"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d659662c-558b-47f9-be26-e749d6cf9afa"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f72d6760-503e-42cc-a70d-6909dbff3315"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "SMS",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "SMS",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("71b7e0ac-b3ff-45fb-804c-101b5f12b54f"), "44b6263f-4156-417f-8352-5e6da1ff023f", "Employee", "EMPLOYEE" },
                    { new Guid("b6050773-12dc-4e83-bffc-b39ceda16db6"), "1fc2c464-9b75-4670-8706-cbb08fb3c9cb", "Admin", "ADMIN" },
                    { new Guid("edd73ff4-4373-437e-97bb-f3800958585b"), "5297d9dc-aac3-47e9-8859-b84604cc91f0", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("71b7e0ac-b3ff-45fb-804c-101b5f12b54f"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b6050773-12dc-4e83-bffc-b39ceda16db6"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("edd73ff4-4373-437e-97bb-f3800958585b"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "SMS",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "SMS",
                table: "Customers");

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("80d977c5-10d6-4b9b-b3f4-ff47e5759519"), "8b96cb46-04a9-4ce7-bf60-405c605bbc7d", "Admin", "ADMIN" },
                    { new Guid("d659662c-558b-47f9-be26-e749d6cf9afa"), "b6d4f48a-f43b-4d40-8ec9-735a626eac71", "Employee", "EMPLOYEE" },
                    { new Guid("f72d6760-503e-42cc-a70d-6909dbff3315"), "1090bc97-ddd5-4f41-af47-310c4e5c5016", "Manager", "MANAGER" }
                });
        }
    }
}
