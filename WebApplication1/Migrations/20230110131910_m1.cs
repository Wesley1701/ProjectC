using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    SSN = table.Column<string>(type: "char(25)", nullable: false),
                    Initials = table.Column<string>(type: "varchar(5)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.SSN);
                });

            migrationBuilder.CreateTable(
                name: "Hours",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<string>(type: "char(30)", nullable: false),
                    ProjectID = table.Column<string>(type: "char(25)", nullable: false),
                    StartTime = table.Column<string>(type: "text", nullable: false),
                    EndTime = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hours", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HourType",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ServiceID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    NumberID = table.Column<string>(type: "char(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false),
                    WorkingOn = table.Column<string>(type: "varchar(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.NumberID);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProjectID = table.Column<string>(type: "char(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorksOn",
                columns: table => new
                {
                    Hours = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeSSN = table.Column<string>(type: "char(30)", nullable: false),
                    ProjectName = table.Column<string>(type: "char(30)", nullable: false),
                    ProjectNumberID = table.Column<string>(type: "char(25)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorksOn", x => x.Hours);
                    table.ForeignKey(
                        name: "FK_WorksOn_Employee_EmployeeSSN",
                        column: x => x.EmployeeSSN,
                        principalTable: "Employee",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorksOn_Project_ProjectNumberID",
                        column: x => x.ProjectNumberID,
                        principalTable: "Project",
                        principalColumn: "NumberID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorksOn_EmployeeSSN",
                table: "WorksOn",
                column: "EmployeeSSN");

            migrationBuilder.CreateIndex(
                name: "IX_WorksOn_ProjectNumberID",
                table: "WorksOn",
                column: "ProjectNumberID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hours");

            migrationBuilder.DropTable(
                name: "HourType");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "WorksOn");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
