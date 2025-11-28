using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TLS.BHL.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Updated_by",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Deleted_by",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Created_by",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Updated_by",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<string>(
                name: "Deleted_by",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<string>(
                name: "Created_by",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");
        }
    }
}
