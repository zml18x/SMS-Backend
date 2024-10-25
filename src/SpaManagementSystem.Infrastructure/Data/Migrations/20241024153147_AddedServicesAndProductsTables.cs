using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedServicesAndProductsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    PurchaseTaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    SaleTaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    StockQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    MinimumStockLevel = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ImgUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Salons_SalonId",
                        column: x => x.SalonId,
                        principalSchema: "SMS",
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ImgUrl = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SalonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Salons_SalonId",
                        column: x => x.SalonId,
                        principalSchema: "SMS",
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                schema: "SMS",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SalonId",
                schema: "SMS",
                table: "Products",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SalonId",
                schema: "SMS",
                table: "Services",
                column: "SalonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_UserId",
                schema: "SMS",
                table: "Employees",
                column: "UserId",
                principalSchema: "SMS",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_UserId",
                schema: "SMS",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "SMS");

            migrationBuilder.DropTable(
                name: "Services",
                schema: "SMS");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                schema: "SMS",
                table: "Employees");

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
        }
    }
}
