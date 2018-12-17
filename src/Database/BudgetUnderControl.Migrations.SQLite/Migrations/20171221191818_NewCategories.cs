using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class NewCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Entertainment' )");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Health' )");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Interest' )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
