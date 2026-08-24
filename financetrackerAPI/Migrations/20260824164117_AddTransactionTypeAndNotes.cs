using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace financetrackerAPI.Migrations
{
    public partial class AddTransactionTypeAndNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "Transactions");
        }
    }
}
