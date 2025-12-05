using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateinventoryreceiptfielfGRNStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GRNStatus",
                table: "InventoryReceipts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GRNStatus",
                table: "InventoryReceipts");
        }
    }
}
