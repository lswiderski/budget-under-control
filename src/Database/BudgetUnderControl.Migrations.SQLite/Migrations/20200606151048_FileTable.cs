using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class FileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "File",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "File",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "File",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileToTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileToTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_FileToTransaction_File",
                        column: x => x.FileId,
                        principalTable: "File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_FileToTransaction_Transaction",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileToTransaction_FileId",
                table: "FileToTransaction",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileToTransaction_TransactionId",
                table: "FileToTransaction",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileToTransaction");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "File");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "File");
        }
    }
}
