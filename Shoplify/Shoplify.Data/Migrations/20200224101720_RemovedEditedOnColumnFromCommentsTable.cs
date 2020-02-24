using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class RemovedEditedOnColumnFromCommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "Comments",
                type: "datetime2",
                nullable: true);
        }
    }
}
