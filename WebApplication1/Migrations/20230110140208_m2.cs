using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectID",
                table: "Service",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(25)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectID",
                table: "Service",
                type: "char(25)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
