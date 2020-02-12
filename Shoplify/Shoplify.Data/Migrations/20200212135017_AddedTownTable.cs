using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class AddedTownTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TownId",
                table: "Advertisements",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_TownId",
                table: "Advertisements",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Towns_TownId",
                table: "Advertisements",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Towns_TownId",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_TownId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Advertisements");
        }
    }
}
