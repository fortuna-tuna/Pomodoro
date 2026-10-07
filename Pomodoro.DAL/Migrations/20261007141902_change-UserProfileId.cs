using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeUserProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_UserProfiles_UserProfileId",
                table: "Statistics");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Statistics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_UserProfiles_UserProfileId",
                table: "Statistics",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_UserProfiles_UserProfileId",
                table: "Statistics");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_UserProfiles_UserProfileId",
                table: "Statistics",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
