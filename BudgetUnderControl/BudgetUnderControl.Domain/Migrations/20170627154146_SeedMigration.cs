using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class SeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO CURRENCY (Code,FullName,Number,Symbol)" +
                                              "VALUES ('PLN', 'Polski Złoty',985, 'zł' )");
            migrationBuilder.Sql("INSERT INTO CURRENCY (Code,FullName,Number,Symbol)" +
                                                "VALUES ('USD', 'United States dollar',840, '$' )");
            migrationBuilder.Sql("INSERT INTO CURRENCY (Code,FullName,Number,Symbol)" +
                                                "VALUES ('EUR', 'Euro',978, '€' )");
            migrationBuilder.Sql("INSERT INTO CURRENCY (Code,FullName,Number,Symbol)" +
                                                "VALUES ('THB', 'Thai baht',764, '฿' )");

            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Food' )");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Transport' )");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Other' )");

            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Salary' )");

            migrationBuilder.Sql("INSERT INTO CATEGORY (Name)" +
                                                "VALUES ('Taxes' )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
