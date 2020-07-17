using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class ExchangeRateSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "ExchangeRate",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExchangeRate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ExchangeRate",
                nullable: true);


            migrationBuilder.Sql("UPDATE [ExchangeRate] SET [ExternalId] = (lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab',abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6))))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "ExchangeRate");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExchangeRate");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ExchangeRate");
        }
    }
}
