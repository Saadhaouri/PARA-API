using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Qr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Debts_DebtID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DebtID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DebtID",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DebtProducts",
                columns: table => new
                {
                    DebtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebtProducts", x => new { x.DebtId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_DebtProducts_Debts_DebtId",
                        column: x => x.DebtId,
                        principalTable: "Debts",
                        principalColumn: "DebtID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DebtProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DebtProducts_ProductId",
                table: "DebtProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "DebtID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DebtID",
                table: "Products",
                column: "DebtID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Debts_DebtID",
                table: "Products",
                column: "DebtID",
                principalTable: "Debts",
                principalColumn: "DebtID");
        }
    }
}
