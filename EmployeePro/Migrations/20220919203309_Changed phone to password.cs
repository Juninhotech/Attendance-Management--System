using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeePro.Migrations
{
    public partial class Changedphonetopassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "WorkersDb");

            migrationBuilder.AddColumn<int>(
                name: "Password",
                table: "WorkersDb",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "WorkersDb");

            migrationBuilder.AddColumn<long>(
                name: "Phone",
                table: "WorkersDb",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
