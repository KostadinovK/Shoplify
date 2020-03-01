using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class AddedUsersAdvertisementsWishlistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersAdvertisementsWishlist",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    AdvertisementId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAdvertisementsWishlist", x => new { x.UserId, x.AdvertisementId });
                    table.ForeignKey(
                        name: "FK_UsersAdvertisementsWishlist_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersAdvertisementsWishlist_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersAdvertisementsWishlist_AdvertisementId",
                table: "UsersAdvertisementsWishlist",
                column: "AdvertisementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersAdvertisementsWishlist");
        }
    }
}
