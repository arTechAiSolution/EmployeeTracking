using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyField.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSelfieColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelfieBase64",
                table: "AttendanceRecords",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelfieBase64",
                table: "AttendanceRecords");
        }
    }
}
