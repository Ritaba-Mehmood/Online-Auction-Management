using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SemesterProj.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminEmailToAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Auth",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "admin@123");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Auth",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "admin2123");
        }
    }
}
