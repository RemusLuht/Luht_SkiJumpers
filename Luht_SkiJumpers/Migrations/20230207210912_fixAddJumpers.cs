using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luht_SkiJumpers.Migrations
{
    public partial class fixAddJumpers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "AddJumpers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Standings",
                table: "AddJumpers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "AddJumpers");

            migrationBuilder.DropColumn(
                name: "Standings",
                table: "AddJumpers");
        }
    }
}
