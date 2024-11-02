using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationsBetweenEmployeesAndServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "EmployeeServices",
                schema: "SMS",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeServices", x => new { x.EmployeesId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_EmployeeServices_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalSchema: "SMS",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeServices_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalSchema: "SMS",
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("751ccc90-246a-479c-9cd7-aa4aeeaa681f"), "9f781995-d242-4658-b96c-038d44f8b274", "Employee", "EMPLOYEE" },
                    { new Guid("89f556d7-672e-4cee-864a-31ef98506af6"), "7246e35c-607c-4cd4-8de6-064bbc1a92b8", "Admin", "ADMIN" },
                    { new Guid("e4787b12-a841-4128-bd8c-bc683ba637b1"), "f49bb069-b491-4112-a6d6-ed7f10bfbc00", "Manager", "MANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServices_ServicesId",
                schema: "SMS",
                table: "EmployeeServices",
                column: "ServicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeServices",
                schema: "SMS");

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("751ccc90-246a-479c-9cd7-aa4aeeaa681f"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89f556d7-672e-4cee-864a-31ef98506af6"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e4787b12-a841-4128-bd8c-bc683ba637b1"));

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
    }
}
