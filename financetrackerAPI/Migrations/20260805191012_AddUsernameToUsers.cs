using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace financetrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Categories_categoryID",
                table: "Budget",
                column: "categoryID",
                principalTable: "Categories",
                principalColumn: "categoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_Categories_categoryID",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");
        }
    }
}
