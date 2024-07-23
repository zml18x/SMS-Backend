using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatedSalonAddressesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "SMS",
                table: "Salons");

            migrationBuilder.CreateTable(
                name: "SalonAddresses",
                schema: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    BuildingNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalonAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalonAddresses_Salons_SalonId",
                        column: x => x.SalonId,
                        principalSchema: "SMS",
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalonAddresses_SalonId",
                schema: "SMS",
                table: "SalonAddresses",
                column: "SalonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalonAddresses",
                schema: "SMS");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                schema: "SMS",
                table: "Salons",
                type: "uuid",
                nullable: true);
        }
    }
}
