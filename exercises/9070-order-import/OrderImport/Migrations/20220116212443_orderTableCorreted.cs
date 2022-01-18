using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderImport.Migrations
{
    public partial class orderTableCorreted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OrderValue",
                table: "Orders",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderValue",
                table: "Orders");
        }
    }
}
