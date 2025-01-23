using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo_03.DAL.Migrations
{
    public partial class AddMigrationAddFileNameToDepartement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Departements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Departements");
        }
    }
}
