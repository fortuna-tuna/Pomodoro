using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addUserProfileAndPomodoroSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PomodoroSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PomodoroDuration = table.Column<int>(type: "int", nullable: false),
                    ShortBreak = table.Column<int>(type: "int", nullable: false),
                    LongBreak = table.Column<int>(type: "int", nullable: false),
                    CountPomodoroBeforeLongBreak = table.Column<int>(type: "int", nullable: false),
                    UserProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PomodoroSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PomodoroSettings_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_UserProfileId",
                table: "Statistics",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PomodoroSettings_UserProfileId",
                table: "PomodoroSettings",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_UserProfiles_UserProfileId",
                table: "Statistics",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_UserProfiles_UserProfileId",
                table: "Statistics");

            migrationBuilder.DropTable(
                name: "PomodoroSettings");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_UserProfileId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Statistics");
        }
    }
}
