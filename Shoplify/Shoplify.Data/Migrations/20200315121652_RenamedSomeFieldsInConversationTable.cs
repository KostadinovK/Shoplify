using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class RenamedSomeFieldsInConversationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversation_AspNetUsers_FirstUserId",
                table: "Conversation");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversation_AspNetUsers_SecondUserId",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Conversation_FirstUserId",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Conversation_SecondUserId",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "FirstUserId",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsArchivedByFirstUser",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsArchivedBySecondUser",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsReadByFirstUser",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsReadBySecondUser",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "SecondUserId",
                table: "Conversation");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Conversation",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedByBuyer",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedBySeller",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByBuyer",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadBySeller",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "Conversation",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsArchivedByBuyer",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsArchivedBySeller",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsReadByBuyer",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsReadBySeller",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Conversation");

            migrationBuilder.AddColumn<string>(
                name: "FirstUserId",
                table: "Conversation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedByFirstUser",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedBySecondUser",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByFirstUser",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadBySecondUser",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecondUserId",
                table: "Conversation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_FirstUserId",
                table: "Conversation",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_SecondUserId",
                table: "Conversation",
                column: "SecondUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversation_AspNetUsers_FirstUserId",
                table: "Conversation",
                column: "FirstUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversation_AspNetUsers_SecondUserId",
                table: "Conversation",
                column: "SecondUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
