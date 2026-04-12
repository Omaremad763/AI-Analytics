using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreateViewFinancialSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW ""View_FinancialSummary"" AS
            SELECT 
                ""TransactionDate""::date AS ""TransactionDate"",
                SUM(""Amount"") AS ""TotalAmount"",
                COUNT(*) AS ""TransactionCount""
            FROM ""FinancialRecords""
            GROUP BY ""TransactionDate""::date;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ""View_FinancialSummary"";");
        }
    }
}
