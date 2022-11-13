using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "Pin");

            migrationBuilder.AddColumn<int>(
                name: "Money",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Pin",
                table: "User",
                newName: "Password");
        }
    }
}
