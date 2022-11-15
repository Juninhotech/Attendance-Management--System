using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeePro.Migrations
{
    public partial class addedanothercolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Phone",
                table: "WorkersDb",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "WorkersDb");
        }
    }
}
