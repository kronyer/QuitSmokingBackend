using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuitSmoking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeRelationOfUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cigarretes_UserCigarreteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCigarreteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserCigarreteId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "UserCigarreteId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserCigarreteId",
                table: "AspNetUsers",
                column: "UserCigarreteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cigarretes_UserCigarreteId",
                table: "AspNetUsers",
                column: "UserCigarreteId",
                principalTable: "Cigarretes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
