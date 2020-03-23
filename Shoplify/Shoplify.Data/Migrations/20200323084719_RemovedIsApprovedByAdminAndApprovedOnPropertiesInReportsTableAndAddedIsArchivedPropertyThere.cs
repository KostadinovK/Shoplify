using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class RemovedIsApprovedByAdminAndApprovedOnPropertiesInReportsTableAndAddedIsArchivedPropertyThere : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedOn",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsApprovedByAdmin",
                table: "Reports");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Reports",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Reports");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedOn",
                table: "Reports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByAdmin",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
