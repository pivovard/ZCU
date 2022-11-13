using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Migrations
{
    public partial class template : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_Payment_paymentId",
                table: "Template");

            migrationBuilder.RenameColumn(
                name: "paymentId",
                table: "Template",
                newName: "PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Template_paymentId",
                table: "Template",
                newName: "IX_Template_PaymentId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Template",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Payment_PaymentId",
                table: "Template",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_Payment_PaymentId",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Template");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Template",
                newName: "paymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Template_PaymentId",
                table: "Template",
                newName: "IX_Template_paymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Payment_paymentId",
                table: "Template",
                column: "paymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
