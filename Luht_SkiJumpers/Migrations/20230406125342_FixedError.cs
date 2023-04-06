using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luht_SkiJumpers.Migrations
{
    public partial class FixedError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AddJumpers",
                table: "AddJumpers");

            migrationBuilder.RenameTable(
                name: "AddJumpers",
                newName: "Jumpers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jumpers",
                table: "Jumpers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Jumpers",
                table: "Jumpers");

            migrationBuilder.RenameTable(
                name: "Jumpers",
                newName: "AddJumpers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddJumpers",
                table: "AddJumpers",
                column: "Id");
        }
    }
}
