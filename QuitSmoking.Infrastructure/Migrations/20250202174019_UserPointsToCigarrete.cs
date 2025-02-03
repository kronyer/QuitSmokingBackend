using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuitSmoking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPointsToCigarrete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cigarretes_AspNetUsers_ApplicationUserId",
                table: "Cigarretes");

            migrationBuilder.DropIndex(
                name: "IX_Cigarretes_ApplicationUserId",
                table: "Cigarretes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Cigarretes");

            migrationBuilder.AddColumn<int>(
                name: "CigarreteId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CigarreteId",
                table: "AspNetUsers",
                column: "CigarreteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cigarretes_CigarreteId",
                table: "AspNetUsers",
                column: "CigarreteId",
                principalTable: "Cigarretes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cigarretes_CigarreteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CigarreteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CigarreteId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Cigarretes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cigarretes_ApplicationUserId",
                table: "Cigarretes",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cigarretes_AspNetUsers_ApplicationUserId",
                table: "Cigarretes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
