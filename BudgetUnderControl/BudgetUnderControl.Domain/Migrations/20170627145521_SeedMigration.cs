using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetUnderControl.Domain
{
    public partial class SeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO CURRENCIES (Code,FullName,Number,Symbol)" +
                                                "VALUES ('PLN', 'Polski Złoty',985, 'zł' )");
            migrationBuilder.Sql("INSERT INTO CURRENCIES (Code,FullName,Number,Symbol)" +
                                                "VALUES ('USD', 'United States dollar',840, '$' )");
            migrationBuilder.Sql("INSERT INTO CURRENCIES (Code,FullName,Number,Symbol)" +
                                                "VALUES ('EUR', 'Euro',978, '€' )");
            migrationBuilder.Sql("INSERT INTO CURRENCIES (Code,FullName,Number,Symbol)" +
                                                "VALUES ('THB', 'Thai baht',764, '฿' )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
