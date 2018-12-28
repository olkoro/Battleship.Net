using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class DbMigration11111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    GameBoardId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    rows = table.Column<int>(nullable: false),
                    cols = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoardId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    AI = table.Column<short>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Ruleses",
                columns: table => new
                {
                    RulesId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CanTouch = table.Column<short>(type: "bit", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruleses", x => x.RulesId);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    value = table.Column<string>(nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    HitOrMiss = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestId);
                });

            migrationBuilder.CreateTable(
                name: "GameboardSquares",
                columns: table => new
                {
                    GameboardSquareId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(nullable: true),
                    GameBoardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameboardSquares", x => x.GameboardSquareId);
                    table.ForeignKey(
                        name: "FK_GameboardSquares_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    ShipId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Health = table.Column<int>(type: "int", nullable: false),
                    GameBoardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.ShipId);
                    table.ForeignKey(
                        name: "FK_Ships_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipsLocations",
                columns: table => new
                {
                    ShipsLocationId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false),
                    ShipId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipsLocations", x => x.ShipsLocationId);
                    table.ForeignKey(
                        name: "FK_ShipsLocations_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    TimeStamp = table.Column<string>(nullable: true),
                    Player1GBId = table.Column<int>(nullable: false),
                    Player2GBId = table.Column<int>(nullable: false),
                    Player1MapId = table.Column<int>(nullable: false),
                    Player2MapId = table.Column<int>(nullable: false),
                    P2Turn = table.Column<short>(type: "bit", nullable: false),
                    SaveId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_States_GameBoards_Player1GBId",
                        column: x => x.Player1GBId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_States_GameBoards_Player1MapId",
                        column: x => x.Player1MapId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_States_GameBoards_Player2GBId",
                        column: x => x.Player2GBId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_States_GameBoards_Player2MapId",
                        column: x => x.Player2MapId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Saves",
                columns: table => new
                {
                    SaveId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    TimeStamp = table.Column<string>(nullable: true),
                    Player1Id = table.Column<int>(nullable: false),
                    Player2Id = table.Column<int>(nullable: false),
                    RulesId = table.Column<int>(nullable: false),
                    LastStateId = table.Column<int>(nullable: false),
                    Replay = table.Column<short>(type: "bit", nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saves", x => x.SaveId);
                    table.ForeignKey(
                        name: "FK_Saves_States_LastStateId",
                        column: x => x.LastStateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Saves_Players_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Saves_Players_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Saves_Ruleses_RulesId",
                        column: x => x.RulesId,
                        principalTable: "Ruleses",
                        principalColumn: "RulesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameboardSquares_GameBoardId",
                table: "GameboardSquares",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_LastStateId",
                table: "Saves",
                column: "LastStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_Player1Id",
                table: "Saves",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_Player2Id",
                table: "Saves",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_RulesId",
                table: "Saves",
                column: "RulesId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_GameBoardId",
                table: "Ships",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipsLocations_ShipId",
                table: "ShipsLocations",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_States_Player1GBId",
                table: "States",
                column: "Player1GBId");

            migrationBuilder.CreateIndex(
                name: "IX_States_Player1MapId",
                table: "States",
                column: "Player1MapId");

            migrationBuilder.CreateIndex(
                name: "IX_States_Player2GBId",
                table: "States",
                column: "Player2GBId");

            migrationBuilder.CreateIndex(
                name: "IX_States_Player2MapId",
                table: "States",
                column: "Player2MapId");

            migrationBuilder.CreateIndex(
                name: "IX_States_SaveId",
                table: "States",
                column: "SaveId");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Saves_SaveId",
                table: "States",
                column: "SaveId",
                principalTable: "Saves",
                principalColumn: "SaveId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_States_GameBoards_Player1GBId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_GameBoards_Player1MapId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_GameBoards_Player2GBId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_GameBoards_Player2MapId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Saves_States_LastStateId",
                table: "Saves");

            migrationBuilder.DropTable(
                name: "GameboardSquares");

            migrationBuilder.DropTable(
                name: "ShipsLocations");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "GameBoards");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Saves");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Ruleses");
        }
    }
}
