using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luht_SkiJumpers.Migrations
{
    public partial class ChangeModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "AddJumpers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Started",
                table: "AddJumpers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "AddJumpers");

            migrationBuilder.DropColumn(
                name: "Started",
                table: "AddJumpers");
        }
    }
}
