using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmployeesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalonId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    EmploymentStatus = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Salons_SalonId",
                        column: x => x.SalonId,
                        principalSchema: "SMS",
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfiles",
                schema: "SMS",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfiles", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeProfiles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "SMS",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SMS",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("06ae96d5-a8f0-487d-9b39-f98b946e5c93"), "45f59a43-c0eb-4d05-94b6-4b312236f475", "Admin", "ADMIN" },
                    { new Guid("740362c7-befd-4437-b68c-0148ca9a31aa"), "3e84ee31-ab88-4b5d-9974-a4eb6eb43aa6", "Employee", "EMPLOYEE" },
                    { new Guid("88f4f814-4409-48fe-bc6c-533c7fe150a6"), "16b7c787-f275-447a-a63e-9c1a3c62cc54", "Manager", "MANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SalonId",
                schema: "SMS",
                table: "Employees",
                column: "SalonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeProfiles",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "SMS");

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("06ae96d5-a8f0-487d-9b39-f98b946e5c93"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("740362c7-befd-4437-b68c-0148ca9a31aa"));

            migrationBuilder.DeleteData(
                schema: "SMS",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("88f4f814-4409-48fe-bc6c-533c7fe150a6"));

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
    }
}
