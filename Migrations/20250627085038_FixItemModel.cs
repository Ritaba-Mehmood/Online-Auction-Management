using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SemesterProj.Migrations
{
    /// <inheritdoc />
    public partial class FixItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Auth_SellerId",
                table: "Items");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Auth_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "Auth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Auth_SellerId",
                table: "Items");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Auth_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "Auth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
