using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatusFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SPM_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SPM_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SPM_Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SPM_Users");
        }
    }
}
