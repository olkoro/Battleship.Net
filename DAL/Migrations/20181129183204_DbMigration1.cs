using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class DbMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardShips");

            migrationBuilder.DropTable(
                name: "SaveStates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardShips",
                columns: table => new
                {
                    BoardShipsId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    GameBoardId = table.Column<int>(nullable: false),
                    ShipId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardShips", x => x.BoardShipsId);
                    table.ForeignKey(
                        name: "FK_BoardShips_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardShips_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaveStates",
                columns: table => new
                {
                    SaveStateId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    SaveId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveStates", x => x.SaveStateId);
                    table.ForeignKey(
                        name: "FK_SaveStates_Saves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "Saves",
                        principalColumn: "SaveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaveStates_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardShips_GameBoardId",
                table: "BoardShips",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardShips_ShipId",
                table: "BoardShips",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveStates_SaveId",
                table: "SaveStates",
                column: "SaveId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveStates_StateId",
                table: "SaveStates",
                column: "StateId");
        }
    }
}
