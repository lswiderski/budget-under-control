using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class AccountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentAccountId",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Account",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentAccountId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Account");
        }
    }
}
