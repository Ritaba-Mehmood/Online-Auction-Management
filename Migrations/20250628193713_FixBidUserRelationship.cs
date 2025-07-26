using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SemesterProj.Migrations
{
    /// <inheritdoc />
    public partial class FixBidUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auth_UserId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auth_UserId",
                table: "Bids",
                column: "UserId",
                principalTable: "Auth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auth_UserId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auth_UserId",
                table: "Bids",
                column: "UserId",
                principalTable: "Auth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
