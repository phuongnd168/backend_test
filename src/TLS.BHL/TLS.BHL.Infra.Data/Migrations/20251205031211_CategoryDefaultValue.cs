using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TLS.BHL.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CategoryDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Updated_by",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Deleted_by",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Created_by",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Updated_by",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Deleted_by",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "phuong",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Created_by",
                table: "Categories",
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
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<string>(
                name: "Deleted_by",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Created_by",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<string>(
                name: "Updated_by",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<string>(
                name: "Deleted_by",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");

            migrationBuilder.AlterColumn<string>(
                name: "Created_by",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "phuong");
        }
    }
}
