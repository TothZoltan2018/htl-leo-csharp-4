using Microsoft.EntityFrameworkCore.Migrations;

namespace TournamentPlanner.Migrations
{
    public partial class Corrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Matches_MatchID",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_MatchID",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MatchID",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "Player1ID",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player2ID",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1ID",
                table: "Matches",
                column: "Player1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2ID",
                table: "Matches",
                column: "Player2ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player1ID",
                table: "Matches",
                column: "Player1ID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player2ID",
                table: "Matches",
                column: "Player2ID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player1ID",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player2ID",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player1ID",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player2ID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player1ID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player2ID",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "MatchID",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_MatchID",
                table: "Players",
                column: "MatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Matches_MatchID",
                table: "Players",
                column: "MatchID",
                principalTable: "Matches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
