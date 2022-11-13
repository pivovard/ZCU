using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Migrations
{
    public partial class dalsizkurvenamigrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_Payment_PaymentId",
                table: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Template_PaymentId",
                table: "Template");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Template",
                newName: "Variable");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Template",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Constant",
                table: "Template",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DestAccount",
                table: "Template",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DestAccountPrefix",
                table: "Template",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestBank",
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
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DestBank",
                table: "Payment",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Constant",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DestAccount",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DestAccountPrefix",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DestBank",
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
                newName: "PaymentId");

            migrationBuilder.AlterColumn<int>(
                name: "DestBank",
                table: "Payment",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Template_PaymentId",
                table: "Template",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Payment_PaymentId",
                table: "Template",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
