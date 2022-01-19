using Microsoft.EntityFrameworkCore.Migrations;

namespace ChuckNorrisExcercise.Migrations
{
    public partial class chIckNorrisColumnCorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChickNorrisId",
                table: "JokeDatas",
                newName: "ChuckNorrisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChuckNorrisId",
                table: "JokeDatas",
                newName: "ChickNorrisId");
        }
    }
}
