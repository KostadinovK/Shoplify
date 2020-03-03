using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class AddedIsBannedBannedOnFieldsAndFieldsForAdPromotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BannedOn",
                table: "Advertisements",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Advertisements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromoted",
                table: "Advertisements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotedOn",
                table: "Advertisements",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotedUntil",
                table: "Advertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannedOn",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsPromoted",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PromotedOn",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PromotedUntil",
                table: "Advertisements");
        }
    }
}
