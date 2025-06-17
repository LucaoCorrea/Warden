using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warden.Migrations
{
    /// <inheritdoc />
    public partial class tbls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Sales",
                newName: "CashbackAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CashbackAvailable",
                table: "Sales",
                newName: "TotalAmount");
        }
    }
}
