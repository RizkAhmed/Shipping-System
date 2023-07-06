using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePKWithSalah_Rizk_muhamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Traders",
                table: "Traders");

            migrationBuilder.DropIndex(
                name: "IX_Traders_AppUserId",
                table: "Traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives");

            migrationBuilder.DropIndex(
                name: "IX_Representatives_AppUserId",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Traders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Representatives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traders",
                table: "Traders",
                column: "AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives",
                column: "AppUserId");

            migrationBuilder.CreateTable(
                name: "TraderSpecialPriceForCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Shippingprice = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderSpecialPriceForCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraderSpecialPriceForCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TraderSpecialPriceForCities_Traders_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Traders",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 7, 5, 23, 27, 46, 802, DateTimeKind.Local).AddTicks(7758));

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2023, 7, 5, 23, 27, 46, 802, DateTimeKind.Local).AddTicks(8185));

            migrationBuilder.CreateIndex(
                name: "IX_TraderSpecialPriceForCities_AppUserId",
                table: "TraderSpecialPriceForCities",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TraderSpecialPriceForCities_CityId",
                table: "TraderSpecialPriceForCities",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraderSpecialPriceForCities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traders",
                table: "Traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Traders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Representatives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traders",
                table: "Traders",
                columns: new[] { "Id", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives",
                columns: new[] { "Id", "AppUserId" });

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 7, 5, 1, 45, 53, 824, DateTimeKind.Local).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2023, 7, 5, 1, 45, 53, 824, DateTimeKind.Local).AddTicks(4567));

            migrationBuilder.CreateIndex(
                name: "IX_Traders_AppUserId",
                table: "Traders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Representatives_AppUserId",
                table: "Representatives",
                column: "AppUserId");
        }
    }
}
