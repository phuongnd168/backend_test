using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TLS.BHL.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_at",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 11, 27, 4, 33, 17, 547, DateTimeKind.Utc).AddTicks(1984));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_at",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2025, 11, 27, 4, 33, 17, 547, DateTimeKind.Utc).AddTicks(1984),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
