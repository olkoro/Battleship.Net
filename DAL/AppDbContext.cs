using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameBoard = Domain.GameBoard;
using Rules = Domain.Rules;
using State = DAL.State;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace DAL
{
    public class AppDbContext : DbContext
    {

        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information, true)
            });

        public DbSet<Save> Saves { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Ship> Ships { get; set; }
        //public DbSet<BoardShips> BoardShips { get; set; }
        public DbSet<Rules> Ruleses { get; set; }
        public DbSet<ShipsLocation> ShipsLocations { get; set; }
        //public DbSet<SaveState> SaveStates { get; set; }
        public DbSet<GameboardSquare> GameboardSquares { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseMySQL(
                    "server=alpha.akaver.com;" +
                    "database=student2018_olkoro_Db;" +
                    "user=student2018;" +
                    "password=student2018");



//                .UseSqlServer(
//                "Server=(localdb)\\mssqllocaldb;" + // server to use
//                "Database=MyDatabase;" + // database to use or create
//                "Trusted_Connection=True;" + // no credentials needed, this is local sql instance
//                "MultipleActiveResultSets=true" // allow multiple  parallel queries
//                );
        }
        public static void SaveToDb()
        {
            Console.WriteLine("Saving...");
            var ctx = new AppDbContext();
            
            foreach (var save in SaveSystem.SavesList)
            {
                var rules = new Rules()
                {
                    CanTouch = Domain.Rules.CanTouch,
                    Rows = Domain.Rules.Boardrows,
                    Columns = Domain.Rules.Boardcolumns
                };
                
                var player1 = new Player(){Name = save[0].P1.Name, AI = save[0].P1.AI};
                var player2 = new Player(){Name = save[0].P2.Name, AI = save[0].P2.AI};
                
                
                var ansave = new Save(){Rules = rules, Player1 = player1,Player2 = player2, Replay = true};

                State laststate = new State();
                var thissave = save;
                if (Domain.Rules.SaveReplays == false)
                {
                    thissave = new List<Domain.State>(){save.Last()};
                    ansave.Replay = false;

                }

//                var p1gb = new GameBoard();
//                var p2gb = new GameBoard();
//                var p1map = new GameBoard();
//                var p2map = new GameBoard();
                foreach (var state in thissave)
                {
                    var p1gb = new GameBoard(){rows = rules.Rows, cols = rules.Columns};
                    var p2gb = new GameBoard(){rows = rules.Rows, cols = rules.Columns};
                    var p1map = new GameBoard(){rows = rules.Rows, cols = rules.Columns};
                    var p2map = new GameBoard(){rows = rules.Rows, cols = rules.Columns};
                    laststate = new State()
                    {
                        Player1GB = p1gb,Player1Map = p1map,
                        Player2GB = p2gb, Player2Map = p2map,P2Turn = state.P2Turn
                    };
                    ctx.States.Add(laststate);
                    
                    //filling in the squares
                    FillSquares(ctx,state.P1.Board,p1gb);
                    FillSquares(ctx,state.P2.Board,p2gb);
                    if (state==thissave.Last())
                    {
                        FillSquares(ctx,state.P1.Map,p1map);
                        FillSquares(ctx,state.P2.Map,p2map);
                        //placing ships
                        FillShips(ctx,state.P1.Board,p1gb);
                        FillShips(ctx,state.P2.Board,p2gb);
                    }          
                    ansave.States.Add(laststate);
                    ctx.SaveChanges();


                }

                ansave.Status = thissave.Last().Status;
                ansave.LastState = laststate;
                ctx.Saves.Add(ansave);
                ctx.SaveChanges();
            }
            SaveSystem.SavesList = new List<List<Domain.State>>();
        }

        public static void FillSquares(AppDbContext ctx,Domain.GameBoard ogboard, GameBoard tofillboard)
        {
            for (int i = 0; i < ogboard.Board.Count; i++)//p1 board
            {
                for (int j = 0; j < ogboard.Board[i].Count; j++)
                {
                    var gameBoardSquare = new GameboardSquare()
                    {
                        GameBoard = tofillboard, x = i, y = j, Value = ogboard.Board[i][j].ToString()
                    };
                    tofillboard.Squares.Add(gameBoardSquare);
                    ctx.GameboardSquares.Add(gameBoardSquare);
                }
            }
        }

        public static void FillShips(AppDbContext ctx, Domain.GameBoard ogboard,GameBoard tofillboard)
        {
            foreach (var ship in ogboard.Ships)//p1
            {
                var lastship = new Ship() {Health = ship.Health, Length = ship.Length};
                //ctx.BoardShips.Add(new BoardShips(){GameBoard = tofillboard, Ship = lastship});
                foreach (var location in ship.Locations)
                {
                    var loc = new ShipsLocation() {y = location[0], x = location[1], Ship = lastship};
                    lastship.ShipsLocations.Add(loc);
                }

                lastship.GameBoard = tofillboard;
                tofillboard.Ships.Add(lastship);
            }
        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            // configure entities
//            
//
//
//            // remove Cascade delete from all the entities <- this has to be at the end
//            foreach (var mutableForeignKey in 
//                modelBuilder.Model.GetEntityTypes()
//                    .Where(e => e.IsOwned() == false)
//                    .SelectMany(e => e.GetForeignKeys()))
//            {
//                mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
//            }
//
//            base.OnModelCreating(modelBuilder);
//        }
    }
}
