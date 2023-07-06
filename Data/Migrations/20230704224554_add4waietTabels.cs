using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class add4waietTabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_governorates_GoverId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_representatives_Branch_BranchId",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_representatives_DiscountTypes_DiscountTypeId",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_representatives_Users_AppUserId",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_representatives_governorates_GovernorateId",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_traders_Cities_CityId",
                table: "traders");

            migrationBuilder.DropForeignKey(
                name: "FK_traders_Users_AppUserId",
                table: "traders");

            migrationBuilder.DropForeignKey(
                name: "FK_traders_governorates_GoverId",
                table: "traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_traders",
                table: "traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_representatives",
                table: "representatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_governorates",
                table: "governorates");

            migrationBuilder.RenameTable(
                name: "traders",
                newName: "Traders");

            migrationBuilder.RenameTable(
                name: "representatives",
                newName: "Representatives");

            migrationBuilder.RenameTable(
                name: "governorates",
                newName: "Governorates");

            migrationBuilder.RenameIndex(
                name: "IX_traders_GoverId",
                table: "Traders",
                newName: "IX_Traders_GoverId");

            migrationBuilder.RenameIndex(
                name: "IX_traders_CityId",
                table: "Traders",
                newName: "IX_Traders_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_traders_AppUserId",
                table: "Traders",
                newName: "IX_Traders_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_representatives_GovernorateId",
                table: "Representatives",
                newName: "IX_Representatives_GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_representatives_DiscountTypeId",
                table: "Representatives",
                newName: "IX_Representatives_DiscountTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_representatives_BranchId",
                table: "Representatives",
                newName: "IX_Representatives_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_representatives_AppUserId",
                table: "Representatives",
                newName: "IX_Representatives_AppUserId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Traders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Traders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Representatives",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traders",
                table: "Traders",
                columns: new[] { "Id", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives",
                columns: new[] { "Id", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Governorates",
                table: "Governorates",
                column: "Id");

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
                name: "IX_Traders_BranchId",
                table: "Traders",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Governorates_GoverId",
                table: "Cities",
                column: "GoverId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_Branch_BranchId",
                table: "Representatives",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_DiscountTypes_DiscountTypeId",
                table: "Representatives",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_Governorates_GovernorateId",
                table: "Representatives",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_Users_AppUserId",
                table: "Representatives",
                column: "AppUserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Traders_Branch_BranchId",
                table: "Traders",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Traders_Cities_CityId",
                table: "Traders",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Traders_Governorates_GoverId",
                table: "Traders",
                column: "GoverId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Traders_Users_AppUserId",
                table: "Traders",
                column: "AppUserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Governorates_GoverId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_Branch_BranchId",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_DiscountTypes_DiscountTypeId",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_Governorates_GovernorateId",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_Users_AppUserId",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Traders_Branch_BranchId",
                table: "Traders");

            migrationBuilder.DropForeignKey(
                name: "FK_Traders_Cities_CityId",
                table: "Traders");

            migrationBuilder.DropForeignKey(
                name: "FK_Traders_Governorates_GoverId",
                table: "Traders");

            migrationBuilder.DropForeignKey(
                name: "FK_Traders_Users_AppUserId",
                table: "Traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traders",
                table: "Traders");

            migrationBuilder.DropIndex(
                name: "IX_Traders_BranchId",
                table: "Traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Governorates",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Traders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Traders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Representatives");

            migrationBuilder.RenameTable(
                name: "Traders",
                newName: "traders");

            migrationBuilder.RenameTable(
                name: "Representatives",
                newName: "representatives");

            migrationBuilder.RenameTable(
                name: "Governorates",
                newName: "governorates");

            migrationBuilder.RenameIndex(
                name: "IX_Traders_GoverId",
                table: "traders",
                newName: "IX_traders_GoverId");

            migrationBuilder.RenameIndex(
                name: "IX_Traders_CityId",
                table: "traders",
                newName: "IX_traders_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Traders_AppUserId",
                table: "traders",
                newName: "IX_traders_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_GovernorateId",
                table: "representatives",
                newName: "IX_representatives_GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_DiscountTypeId",
                table: "representatives",
                newName: "IX_representatives_DiscountTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_BranchId",
                table: "representatives",
                newName: "IX_representatives_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_AppUserId",
                table: "representatives",
                newName: "IX_representatives_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_traders",
                table: "traders",
                columns: new[] { "Id", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_representatives",
                table: "representatives",
                columns: new[] { "Id", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_governorates",
                table: "governorates",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_governorates_GoverId",
                table: "Cities",
                column: "GoverId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_representatives_Branch_BranchId",
                table: "representatives",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_representatives_DiscountTypes_DiscountTypeId",
                table: "representatives",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_representatives_Users_AppUserId",
                table: "representatives",
                column: "AppUserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_representatives_governorates_GovernorateId",
                table: "representatives",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_traders_Cities_CityId",
                table: "traders",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_traders_Users_AppUserId",
                table: "traders",
                column: "AppUserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_traders_governorates_GoverId",
                table: "traders",
                column: "GoverId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
