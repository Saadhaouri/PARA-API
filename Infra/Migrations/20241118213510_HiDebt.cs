using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class HiDebt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Debts",
                newName: "LastDatePayee");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Debts",
                newName: "DateDebt");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Debts",
                newName: "total");

            migrationBuilder.AddColumn<decimal>(
                name: "avance",
                table: "Debts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "rest",
                table: "Debts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avance",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "rest",
                table: "Debts");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "Debts",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "LastDatePayee",
                table: "Debts",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "DateDebt",
                table: "Debts",
                newName: "Date");
        }
    }
}
