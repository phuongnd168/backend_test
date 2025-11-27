using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TLS.BHL.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "statusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });
            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Name", "Created_by", "Updated_by", "Deleted_by", "Deleted" },
                values: new object[] { 0, "Default", "system", "system", "system", false }
            );
            migrationBuilder.CreateIndex(
                name: "IX_Orders_statusId",
                table: "Orders",
                column: "statusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Status_statusId",
                table: "Orders",
                column: "statusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Status_statusId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Orders_statusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "statusId",
                table: "Orders");
        }
    }
}
