using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class Syncdrop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Synchronization");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Synchronization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Component = table.Column<byte>(type: "INTEGER", nullable: false),
                    ComponentId = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
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
    }
}
