using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class Seed_ModifiedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Category] SET [ModifiedOn] = DATETIME('now') WHERE [ModifiedOn] IS NULL");
            migrationBuilder.Sql("UPDATE [Account] SET [ModifiedOn] = DATETIME('now') WHERE [ModifiedOn] IS NULL");
            migrationBuilder.Sql("UPDATE [AccountGroup] SET [ModifiedOn] = DATETIME('now') WHERE [ModifiedOn] IS NULL");
            migrationBuilder.Sql("UPDATE [Tag] SET [ModifiedOn] = DATETIME('now') WHERE [ModifiedOn] IS NULL");
            migrationBuilder.Sql("UPDATE [Transaction] SET [ModifiedOn] = [CreatedOn] WHERE [ModifiedOn] IS NULL");
            migrationBuilder.Sql("UPDATE [Transfer] SET [ModifiedOn] = DATETIME('now') WHERE [ModifiedOn] IS NULL");
            migrationBuilder.Sql("UPDATE [User] SET [ModifiedOn] = [CreatedAt] WHERE [ModifiedOn] IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
