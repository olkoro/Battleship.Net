using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Saves_GameBoards_Player1GBId",
                table: "Saves");

            migrationBuilder.DropForeignKey(
                name: "FK_Saves_GameBoards_Player1MapId",
                table: "Saves");

            migrationBuilder.DropForeignKey(
                name: "FK_Saves_GameBoards_Player2GBId",
                table: "Saves");

            migrationBuilder.DropForeignKey(
                name: "FK_Saves_GameBoards_Player2MapId",
                table: "Saves");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Saves_Player1GBId",
                table: "Saves");

            migrationBuilder.DropIndex(
                name: "IX_Saves_Player1MapId",
                table: "Saves");

            migrationBuilder.DropIndex(
                name: "IX_Saves_Player2GBId",
                table: "Saves");

            migrationBuilder.DropIndex(
                name: "IX_Saves_Player2MapId",
                table: "Saves");

            migrationBuilder.DropColumn(
                name: "P2Turn",
                table: "Saves");

            migrationBuilder.DropColumn(
                name: "Player1GBId",
                table: "Saves");

            migrationBuilder.DropColumn(
                name: "Player1MapId",
                table: "Saves");

            migrationBuilder.DropColumn(
                name: "Player2GBId",
                table: "Saves");

            migrationBuilder.DropColumn(
                name: "Player2MapId",
                table: "Saves");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "P2Turn",
                table: "Saves",
                type: "bit",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "Player1GBId",
                table: "Saves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player1MapId",
                table: "Saves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2GBId",
                table: "Saves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2MapId",
                table: "Saves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    HitOrMiss = table.Column<string>(nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Saves_Player1GBId",
                table: "Saves",
                column: "Player1GBId");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_Player1MapId",
                table: "Saves",
                column: "Player1MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_Player2GBId",
                table: "Saves",
                column: "Player2GBId");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_Player2MapId",
                table: "Saves",
                column: "Player2MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Saves_GameBoards_Player1GBId",
                table: "Saves",
                column: "Player1GBId",
                principalTable: "GameBoards",
                principalColumn: "GameBoardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saves_GameBoards_Player1MapId",
                table: "Saves",
                column: "Player1MapId",
                principalTable: "GameBoards",
                principalColumn: "GameBoardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saves_GameBoards_Player2GBId",
                table: "Saves",
                column: "Player2GBId",
                principalTable: "GameBoards",
                principalColumn: "GameBoardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saves_GameBoards_Player2MapId",
                table: "Saves",
                column: "Player2MapId",
                principalTable: "GameBoards",
                principalColumn: "GameBoardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
