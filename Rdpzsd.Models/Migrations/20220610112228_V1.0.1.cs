using Microsoft.EntityFrameworkCore.Migrations;

namespace Rdpzsd.Models.Migrations
{
    public partial class V101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "firstcriteriaacceptedcount",
                table: "specialityimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "firstcriteriacount",
                table: "specialityimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "secondcriteriaacceptedcount",
                table: "specialityimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "secondcriteriacount",
                table: "specialityimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "firstcriteriaacceptedcount",
                table: "personimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "firstcriteriacount",
                table: "personimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "secondcriteriaacceptedcount",
                table: "personimport",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "secondcriteriacount",
                table: "personimport",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firstcriteriaacceptedcount",
                table: "specialityimport");

            migrationBuilder.DropColumn(
                name: "firstcriteriacount",
                table: "specialityimport");

            migrationBuilder.DropColumn(
                name: "secondcriteriaacceptedcount",
                table: "specialityimport");

            migrationBuilder.DropColumn(
                name: "secondcriteriacount",
                table: "specialityimport");

            migrationBuilder.DropColumn(
                name: "firstcriteriaacceptedcount",
                table: "personimport");

            migrationBuilder.DropColumn(
                name: "firstcriteriacount",
                table: "personimport");

            migrationBuilder.DropColumn(
                name: "secondcriteriaacceptedcount",
                table: "personimport");

            migrationBuilder.DropColumn(
                name: "secondcriteriacount",
                table: "personimport");
        }
    }
}
