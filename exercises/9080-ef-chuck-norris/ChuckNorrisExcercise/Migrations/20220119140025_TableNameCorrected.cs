using Microsoft.EntityFrameworkCore.Migrations;

namespace ChuckNorrisExcercise.Migrations
{
    public partial class TableNameCorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.CreateTable(
                name: "JokeDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChickNorrisId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Joke = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JokeDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JokeDatas");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChickNorrisId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Joke = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }
    }
}
