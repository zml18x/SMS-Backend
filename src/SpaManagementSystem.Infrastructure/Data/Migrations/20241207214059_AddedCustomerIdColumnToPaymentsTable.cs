using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerIdColumnToPaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "SMS",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3ee9f07a-f66d-4aeb-a0cd-bccb4a552267"), "745a1777-2502-4fbc-aaa6-698f2f91d60e", "Admin", "ADMIN" },
                    { new Guid("67a894b7-3b7e-42a2-b449-668cde1956eb"), "c2e13888-d82b-47a9-824d-fe8874fbb4e9", "Manager", "MANAGER" },
                    { new Guid("74dd83f4-c9e6-4ac2-a111-8fb9b9310123"), "130e065d-ec47-4e53-bb85-5e637c704db1", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CustomerId",
                schema: "SMS",
                table: "Payments",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Customers_CustomerId",
                schema: "SMS",
                table: "Payments",
                column: "CustomerId",
                principalSchema: "SMS",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Customers_CustomerId",
                schema: "SMS",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CustomerId",
                schema: "SMS",
                table: "Payments");

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3ee9f07a-f66d-4aeb-a0cd-bccb4a552267"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("67a894b7-3b7e-42a2-b449-668cde1956eb"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("74dd83f4-c9e6-4ac2-a111-8fb9b9310123"));

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "SMS",
                table: "Payments");

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
    }
}
