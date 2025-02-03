using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuitSmoking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCigarreteIdToSmokingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CigarreteId",
                table: "SmokingHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SmokingHistories_CigarreteId",
                table: "SmokingHistories",
                column: "CigarreteId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmokingHistories_Cigarretes_CigarreteId",
                table: "SmokingHistories",
                column: "CigarreteId",
                principalTable: "Cigarretes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmokingHistories_Cigarretes_CigarreteId",
                table: "SmokingHistories");

            migrationBuilder.DropIndex(
                name: "IX_SmokingHistories_CigarreteId",
                table: "SmokingHistories");

            migrationBuilder.DropColumn(
                name: "CigarreteId",
                table: "SmokingHistories");
        }
    }
}
