using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SnmpManager.API.Data.Migrations
{
    public partial class CreateTableAgents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Watchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    Mib = table.Column<string>(nullable: true),
                    UpdatesEvery = table.Column<int>(nullable: false),
                    Method = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Watchers");
        }
    }
}