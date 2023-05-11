using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimiiformesWebApplication.Data.Migrations
{
    public partial class PersonVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Person",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Person");
        }
    }
}
