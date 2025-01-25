using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Test11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtProducts_Debts_DebtId",
                table: "DebtProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtProducts_Products_ProductId",
                table: "DebtProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_Products_ProductId",
                table: "ProductPromotions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_Promotions_PromotionId",
                table: "ProductPromotions");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtProducts_Debts_DebtId",
                table: "DebtProducts",
                column: "DebtId",
                principalTable: "Debts",
                principalColumn: "DebtID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtProducts_Products_ProductId",
                table: "DebtProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_Products_ProductId",
                table: "ProductPromotions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_Promotions_PromotionId",
                table: "ProductPromotions",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "PromotionID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtProducts_Debts_DebtId",
                table: "DebtProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtProducts_Products_ProductId",
                table: "DebtProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_Products_ProductId",
                table: "ProductPromotions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_Promotions_PromotionId",
                table: "ProductPromotions");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtProducts_Debts_DebtId",
                table: "DebtProducts",
                column: "DebtId",
                principalTable: "Debts",
                principalColumn: "DebtID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtProducts_Products_ProductId",
                table: "DebtProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_Products_ProductId",
                table: "ProductPromotions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_Promotions_PromotionId",
                table: "ProductPromotions",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "PromotionID");
        }
    }
}
