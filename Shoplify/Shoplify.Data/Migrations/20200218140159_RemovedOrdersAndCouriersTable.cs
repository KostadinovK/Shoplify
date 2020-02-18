using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shoplify.Web.Data.Migrations
{
    public partial class RemovedOrdersAndCouriersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_Orders_CurrentOrderId1",
                table: "Couriers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Couriers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuyerAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BuyerFirstName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuyerLastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CourierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EstimatedDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal", nullable: false),
                    SellerAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SellerFirstName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerLastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentOrderId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    HiredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Couriers_Orders_CurrentOrderId1",
                        column: x => x.CurrentOrderId1,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Couriers_CurrentOrderId1",
                table: "Couriers",
                column: "CurrentOrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Couriers_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
