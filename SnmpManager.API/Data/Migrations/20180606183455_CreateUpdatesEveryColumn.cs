using Microsoft.EntityFrameworkCore.Migrations;

namespace SnmpManager.API.Data.Migrations
{
    public partial class CreateUpdatesEveryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatesEvery",
                table: "Watchers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatesEvery",
                table: "Watchers");
        }
    }
}
