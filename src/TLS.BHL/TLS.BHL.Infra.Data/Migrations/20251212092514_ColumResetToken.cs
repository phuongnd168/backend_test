using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TLS.BHL.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ColumResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiredAt",
                table: "ForgotPassword",
                newName: "ExpiredOtpAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredResetTokenAt",
                table: "ForgotPassword",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "resetToken",
                table: "ForgotPassword",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredResetTokenAt",
                table: "ForgotPassword");

            migrationBuilder.DropColumn(
                name: "resetToken",
                table: "ForgotPassword");

            migrationBuilder.RenameColumn(
                name: "ExpiredOtpAt",
                table: "ForgotPassword",
                newName: "ExpiredAt");
        }
    }
}
