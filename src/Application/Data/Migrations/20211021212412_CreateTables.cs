using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Application.Data.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guitar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GuitarType = table.Column<int>(type: "int", nullable: false),
                    MaxNumberOfStrings = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guitar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuitarString",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    Gauge = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    Tuning = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    GuitarId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuitarString", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuitarString_Guitar_GuitarId",
                        column: x => x.GuitarId,
                        principalTable: "Guitar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuitarString_GuitarId",
                table: "GuitarString",
                column: "GuitarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuitarString");

            migrationBuilder.DropTable(
                name: "Guitar");
        }
    }
}
