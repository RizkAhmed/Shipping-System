using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTraderRepresentiteveCityGov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliverTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GoverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_governorates_GoverId",
                        column: x => x.GoverId,
                        principalTable: "governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "representatives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyPercentageOfOrder = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    DiscountTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_representatives", x => new { x.Id, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_representatives_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_representatives_DiscountTypes_DiscountTypeId",
                        column: x => x.DiscountTypeId,
                        principalTable: "DiscountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_representatives_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalSchema: "security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_representatives_governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "traders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialPickupCost = table.Column<int>(type: "int", nullable: false),
                    TraderTaxForRejectedOrders = table.Column<int>(type: "int", nullable: false),
                    GoverId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_traders", x => new { x.Id, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_traders_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_traders_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalSchema: "security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_traders_governorates_GoverId",
                        column: x => x.GoverId,
                        principalTable: "governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 7, 5, 0, 47, 34, 490, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2023, 7, 5, 0, 47, 34, 490, DateTimeKind.Local).AddTicks(2181));

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GoverId",
                table: "Cities",
                column: "GoverId");

            migrationBuilder.CreateIndex(
                name: "IX_representatives_AppUserId",
                table: "representatives",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_representatives_BranchId",
                table: "representatives",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_representatives_DiscountTypeId",
                table: "representatives",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_representatives_GovernorateId",
                table: "representatives",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_traders_AppUserId",
                table: "traders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_traders_CityId",
                table: "traders",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_traders_GoverId",
                table: "traders",
                column: "GoverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliverTypes");

            migrationBuilder.DropTable(
                name: "representatives");

            migrationBuilder.DropTable(
                name: "traders");

            migrationBuilder.DropTable(
                name: "DiscountTypes");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "governorates");

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 7, 3, 4, 55, 55, 874, DateTimeKind.Local).AddTicks(2898));

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2023, 7, 3, 4, 55, 55, 874, DateTimeKind.Local).AddTicks(2980));
        }
    }
}
