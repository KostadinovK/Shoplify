using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class AddedIsArchivedByUserBoolsToConversationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Conversation");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedByFirstUser",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedBySecondUser",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByFirstUser",
                table: "Conversation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadBySecondUser",
                table: "Conversation",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
