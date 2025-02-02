using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuitSmoking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserCigarreteToBeConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmokingHistories_Cigarretes_CigarretesId",
                table: "SmokingHistories");

            migrationBuilder.DropIndex(
                name: "IX_SmokingHistories_CigarretesId",
                table: "SmokingHistories");

            migrationBuilder.DropColumn(
                name: "CigarretesId",
                table: "SmokingHistories");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cigarretes_UserCigarreteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCigarreteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserCigarreteId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CigarretesId",
                table: "SmokingHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SmokingHistories_CigarretesId",
                table: "SmokingHistories",
                column: "CigarretesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmokingHistories_Cigarretes_CigarretesId",
                table: "SmokingHistories",
                column: "CigarretesId",
                principalTable: "Cigarretes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
