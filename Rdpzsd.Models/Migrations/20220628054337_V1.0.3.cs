using Microsoft.EntityFrameworkCore.Migrations;

namespace Rdpzsd.Models.Migrations
{
    public partial class V103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "subordinateid",
                table: "personlotidnumber",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subordinateid",
                table: "personlotidnumber");
        }
    }
}
