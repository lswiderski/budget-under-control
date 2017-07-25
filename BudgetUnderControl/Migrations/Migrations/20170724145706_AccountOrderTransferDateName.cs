using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class AccountOrderTransferDateName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Transaction",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Account",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Transaction",
                newName: "CreatedOn");
        }
    }
}
