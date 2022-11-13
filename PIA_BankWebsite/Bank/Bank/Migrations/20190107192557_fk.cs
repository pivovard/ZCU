using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Migrations
{
    public partial class fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Constant",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Specific",
                table: "Template");

            migrationBuilder.RenameColumn(
                name: "Variable",
                table: "Template",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "paymentId",
                table: "Template",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FromAccount",
                table: "Payment",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Payment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Template_UserId",
                table: "Template",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_paymentId",
                table: "Template",
                column: "paymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_User_UserId",
                table: "Template",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Payment_paymentId",
                table: "Template",
                column: "paymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_User_UserId",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_Payment_paymentId",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_UserId",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_paymentId",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Payment_UserId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "paymentId",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Template",
                newName: "Variable");

            migrationBuilder.AddColumn<int>(
                name: "Account",
                table: "Template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bank",
                table: "Template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Constant",
                table: "Template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Template",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Specific",
                table: "Template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FromAccount",
                table: "Payment",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
