using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoShare.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Photo",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_ApplicationUserId",
                table: "Photo",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_AspNetUsers_ApplicationUserId",
                table: "Photo",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_AspNetUsers_ApplicationUserId",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_ApplicationUserId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Photo");
        }
    }
}
