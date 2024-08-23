using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRefreshTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("14a4947f-74ee-492f-b190-441b2400438c"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("36adb3bf-0099-4f33-bb86-0c2d64b6b1ca"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("92622558-b8e2-4285-b011-0fa169b3af32"));

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("129a2c5b-c667-4166-87b8-8d63aaa639d0"), "b46f8d15-2c9d-4ed3-a366-ff5ad985f411", "Admin", "ADMIN" },
                    { new Guid("39665e64-352e-4165-9235-88d3d25f1283"), "c4a19780-6dff-49a1-94fa-6e740f7ee070", "Employee", "EMPLOYEE" },
                    { new Guid("e336f5bc-8fcb-47c4-84a2-88c8a1d3624f"), "a5faed77-04f1-4db0-b287-5b6da3de5509", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "SMS");

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("129a2c5b-c667-4166-87b8-8d63aaa639d0"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("39665e64-352e-4165-9235-88d3d25f1283"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e336f5bc-8fcb-47c4-84a2-88c8a1d3624f"));

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("14a4947f-74ee-492f-b190-441b2400438c"), "7900064d-43a8-4603-b4f9-dd8dbf0ed726", "Admin", "ADMIN" },
                    { new Guid("36adb3bf-0099-4f33-bb86-0c2d64b6b1ca"), "9928810d-0f2c-4e54-83d5-215f0da63e24", "Manager", "MANAGER" },
                    { new Guid("92622558-b8e2-4285-b011-0fa169b3af32"), "d2eb539e-b6d0-41bd-8264-e99f1ec37e8f", "Employee", "EMPLOYEE" }
                });
        }
    }
}
