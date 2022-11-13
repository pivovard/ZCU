using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Migrations
{
    public partial class zebyuzkonecne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Template");

            migrationBuilder.AlterColumn<int>(
                name: "DestBank",
                table: "Payment",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Template",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DestBank",
                table: "Payment",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
