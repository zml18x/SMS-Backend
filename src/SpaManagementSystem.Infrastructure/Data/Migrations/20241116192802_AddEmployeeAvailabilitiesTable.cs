using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAvailabilitiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "EmployeeAvailabilities",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAvailabilities_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "SMS",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAvailabilityHours",
                schema: "SMS",
                columns: table => new
                {
                    EmployeeAvailabilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<TimeSpan>(type: "interval", nullable: false),
                    End = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAvailabilityHours", x => new { x.EmployeeAvailabilityId, x.Id });
                    table.ForeignKey(
                        name: "FK_EmployeeAvailabilityHours_EmployeeAvailabilities_EmployeeAv~",
                        column: x => x.EmployeeAvailabilityId,
                        principalSchema: "SMS",
                        principalTable: "EmployeeAvailabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4b753fdb-1917-4f57-aed9-fe4afff4f992"), "aac54872-27a6-472e-947f-dfa662a172a2", "Employee", "EMPLOYEE" },
                    { new Guid("86b3c9f5-75e6-4d99-917b-d82df97b0f31"), "48cf3c07-e6fb-42d2-a0c8-06d44d309d51", "Manager", "MANAGER" },
                    { new Guid("ce6c5941-6017-47b7-a8dd-69f849f60759"), "601b61af-bb53-4ac6-ba10-f33ee6060b10", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAvailabilities_EmployeeId",
                schema: "SMS",
                table: "EmployeeAvailabilities",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAvailabilityHours",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "EmployeeAvailabilities",
                schema: "SMS");

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4b753fdb-1917-4f57-aed9-fe4afff4f992"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("86b3c9f5-75e6-4d99-917b-d82df97b0f31"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ce6c5941-6017-47b7-a8dd-69f849f60759"));

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
        }
    }
}
