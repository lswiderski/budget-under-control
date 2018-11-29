using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetUnderControl.Migrations.SqlServer.Migrations
{
    public partial class set_same_external_ids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = 'CB80CF16-02DB-47BA-9EE7-5D9E78A695DB' WHERE [Name] = 'Food'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = '3A458C42-F302-4664-86E5-7AC9598CD511' WHERE [Name] = 'Transport'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = 'E457E4D5-83F5-481E-A34D-6B983FB37756' WHERE [Name] = 'Other'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = '153253BD-A5E6-47B6-889F-CFCD3146CBAE' WHERE [Name] = 'Salary'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = '1D268617-00A4-458B-A356-B86CC9AED482' WHERE [Name] = 'Taxes'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = '4313C995-993A-4E16-9A7E-F0E5120917DE' WHERE [Name] = 'Entertainment'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = '28F813D3-1D26-44C1-A3B4-88B17FB9963C' WHERE [Name] = 'Health'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = 'DDEF31C7-7FA4-4E0C-A456-55B85353B46D' WHERE [Name] = 'Interest'");
            migrationBuilder.Sql("UPDATE [Category] SET [ExternalId] = '3956D5EA-A428-453B-9F68-CF486DD942FF' WHERE [Name] = 'Home'");

            migrationBuilder.Sql("UPDATE [ACCOUNTGROUP] SET [ExternalId] = '5F9C5721-24E8-4366-8E81-BBB09B20F265' WHERE [Name] = 'Cash'");
            migrationBuilder.Sql("UPDATE [ACCOUNTGROUP] SET [ExternalId] = '34A216A1-C9CB-4012-9344-8C39FF5D5B4C' WHERE [Name] = 'Account'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
