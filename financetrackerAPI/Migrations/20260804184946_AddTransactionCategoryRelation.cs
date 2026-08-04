using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace financetrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionCategoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryID",
                table: "Transactions",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryID",
                table: "Transactions",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryID",
                table: "Transactions");
        }
    }
}
