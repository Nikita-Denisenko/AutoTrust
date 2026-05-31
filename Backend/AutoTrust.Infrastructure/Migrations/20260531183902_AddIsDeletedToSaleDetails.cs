using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoTrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToSaleDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SaleDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BuyDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BuyDetails");
        }
    }
}
