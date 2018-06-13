using Microsoft.EntityFrameworkCore.Migrations;

namespace SnmpManager.API.Data.Migrations
{
    public partial class RemoveUpdatesEveryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatesEvery",
                table: "Watchers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatesEvery",
                table: "Watchers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
