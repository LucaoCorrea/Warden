using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warden.Migrations
{
    /// <inheritdoc />
    public partial class tbls3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Users_UserId",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_UserId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Chat");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Chat");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Chat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_UserId",
                table: "Chat",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Users_UserId",
                table: "Chat",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
