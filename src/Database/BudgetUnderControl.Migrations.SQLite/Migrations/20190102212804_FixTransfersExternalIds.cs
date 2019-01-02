using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class FixTransfersExternalIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Transaction] SET [ExternalId] = (lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab',abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))) where EXISTS(SELECT 1 FROM [Transfer] ft WHERE ft.FromTransactionId = [Transaction].[Id] ) or EXISTS(SELECT 1 FROM [Transfer] tt WHERE tt.ToTransactionId = [Transaction].[Id] )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
