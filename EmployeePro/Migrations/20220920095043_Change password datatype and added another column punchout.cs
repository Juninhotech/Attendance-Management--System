using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeePro.Migrations
{
    public partial class Changepassworddatatypeandaddedanothercolumnpunchout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "WorkersDb",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "PunchOut",
                table: "EmployeeDb",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PunchOut",
                table: "EmployeeDb");

            migrationBuilder.AlterColumn<int>(
                name: "Password",
                table: "WorkersDb",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
