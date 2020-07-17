using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class Syncrecreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Synchronization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastSyncAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Component = table.Column<byte>(nullable: false),
                    ComponentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synchronization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Synchronization_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Synchronization_UserId",
                table: "Synchronization",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Synchronization");
        }
    }
}
