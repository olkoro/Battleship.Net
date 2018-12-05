using System;
using System.Collections.Generic;

namespace Domain
{
    public class AI
    {
        public static int[] randomCoords(GameBoard board)
        {
            Random rnd = new Random();
            int[] coords = new int[2];
            bool ok = false;
            while (true)
            {
                int row = rnd.Next(0, Rules.Boardrows);
                int column = rnd.Next(0, Rules.Boardcolumns);
                coords[0] = row;
                coords[1] = column;
                if (board.Board[row][column] == BoardSquareState.Empty)
                {
                    return coords;
                }
            }
            return coords;
        }

        public static bool CheckPlace(GameBoard Board, int[] coords,int shipLen,bool rotation)
        {
            for (int i = 0; i < shipLen; i++) //checking if the ship itself will fit inside the area
            {
                if (rotation == true)
                {
                    try
                    {
                        if (Board.Board[coords[0] + i][coords[1]] == BoardSquareState.Ship)
                        {
                            return false;
                        }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        if (Board.Board[coords[0]][coords[1] + i] == BoardSquareState.Ship)
                        {
                            return false;
                        }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            if (Rules.CanTouch == false)
            {
                for (int i = 0; i < shipLen + 2; i++)//checking around it
                {
                    if (rotation == true)
                    {
                        for (int j = 0; j < 3; j++)
                            try
                            {
                                if (Board.Board[coords[0] + i - 1][coords[1] + j - 1] == BoardSquareState.Ship)
                                {
                                    return false;
                                }
                            }catch (System.ArgumentOutOfRangeException)
                            {
                            }
                    }
                    else
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            try
                            {
                                if (Board.Board[coords[0] + j - 1][coords[1] + i - 1] == BoardSquareState.Ship)
                                {
                                    return false;
                                }
                            }catch (System.ArgumentOutOfRangeException)
                            {
                            }
                        }
                    }
                }
            }
            
            return true;
        }
        
        public static bool AIPlacing(List<Ship> ships,GameBoard board)
        {
            bool rotation = false;
            int iterations = 0;
            while (ships.Count > 0)
            {
                iterations++;
                if (iterations > 200)
                {
                    return false;
                }
                for (int i = 0; i < ships.Count; i++)
                {
                    if (rotation)
                    {
                        rotation = false;
                    }
                    else
                    {
                        rotation = true;}
                    var coords = randomCoords(board);
                    if (CheckPlace(board, coords,ships[i].Length,rotation))
                    {
                        SetPlace(board,coords,ships[i].Length,rotation);
                        ships.Remove(ships[i]);
                    }
                }
            }
            return true;
        }

        public static bool CheckDead(GameBoard board, int[] coords,GameBoard map)
        {
            var ship = FindShip(board, coords);
            if (ship.Health == 0)//dead
            {
                foreach (var point in ship.Locations)//marking dead
                {
                    board.Board[point[0]][point[1]] = BoardSquareState.Dead;
                    map.Board[point[0]][point[1]] = BoardSquareState.Dead;
                }

                if (Rules.CanTouch == false)
                {
                    foreach (var point in ship.Locations)//marking misses
                    {
                        try
                        {
                            if (map.Board[point[0] - 1][point[1]] != BoardSquareState.Dead) //up
                            {
                                map.Board[point[0] - 1][point[1]] = BoardSquareState.Miss;
                            }
                        }
                        catch
                        {
                            
                        }try
                        {
    
                            if (map.Board[point[0] + 1][point[1]] != BoardSquareState.Dead) //down
                            {
                                map.Board[point[0] + 1][point[1]] = BoardSquareState.Miss;
                            }
                        }
                        catch
                        {
                            
                        }
                        try
                        { 
    
                            if (map.Board[point[0]][point[1] + 1] != BoardSquareState.Dead) //right
                            {
                                map.Board[point[0]][point[1] + 1] = BoardSquareState.Miss;
                            }
                        }
                        catch
                        {
                            
                        }
                        try
                        {
                            if (map.Board[point[0]][point[1] - 1] != BoardSquareState.Dead) //right
                            {
                                map.Board[point[0]][point[1] - 1] = BoardSquareState.Miss;
                            }
                        }
                        catch
                        {
                            
                        }try{
                            map.Board[point[0] - 1][point[1] - 1] = BoardSquareState.Miss; //topright
                        }
                        catch
                        {
                            
                        }try{
                            map.Board[point[0] + 1][point[1] - 1] = BoardSquareState.Miss; //botright
                        }
                        catch
                        {
                            
                        }try{
                            map.Board[point[0] - 1][point[1] + 1] = BoardSquareState.Miss; //topleft
                        }
                        catch
                        {
                            
                        }try{
                            map.Board[point[0] + 1][point[1] + 1] = BoardSquareState.Miss; //botleft
                        }
                        catch
                        {
                            
                        }
    
                    }
                }

                board.Ships.Remove(ship);
                return true;
            }
            return false;
            
        }

        public static Ship FindShip(GameBoard board, int[] coords)
        {
            foreach (var ship in board.Ships)
            {
                Console.WriteLine(ship.Length);
                foreach (var point in ship.Locations)
                {
                    if (point[0] == coords[0] && point[1] == coords[1])
                    {
                        return ship;
                    }
                }
            }
            Console.WriteLine("could not find a ship");
            return null;
        }
        
        public static void SetPlace(GameBoard board, int[] coords,int shipLen,bool rotation)
        {
            var row = coords[0];
            var column = coords[1];
            var ship = new Ship(shipLen);
            for (int i = 0; i < shipLen; i++)
            {    
                if (rotation == true)
                {
                    ship.Locations.Add(new []{row+i,column});
                    board.Board[row+i][column] = BoardSquareState.Ship;
                }
                else
                {
                    ship.Locations.Add(new []{row,column+i});
                    board.Board[row][column+i] = BoardSquareState.Ship;
                }
            }
            board.Ships.Add(ship);
        }

        public static string AIShoot(GameBoard board, GameBoard map)
        {
            Domain.Orientation orientation = Orientation.Unknown;
            Random rnd = new Random();
            for (int i = 0; i < map.Board.Count; i++)//TOdo: does not work correctly
            {
                for (int j = 0; j < map.Board[i].Count; j++)//look at board 
                {
                    if (map.Board[i][j] == BoardSquareState.Hit)//found a hit
                    {
                        //check for other hits
                        //up
                        try
                        {
                            if (map.Board[i-1][j] == BoardSquareState.Hit)
                            {
                                orientation = Orientation.Vertical;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        //down
                        try
                        {
                            if (map.Board[i+1][j] == BoardSquareState.Hit)
                            {
                                orientation = Orientation.Vertical;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        //right
                        try
                        {
                            if (map.Board[i][j+1] == BoardSquareState.Hit)
                            {
                                orientation = Orientation.Horisontal;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        //left
                        try
                        {
                            if (map.Board[i][j-1] == BoardSquareState.Hit)
                            {
                                orientation = Orientation.Horisontal;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        //guessing orientation
                        if (orientation == Orientation.Unknown)
                        {
                            if (rnd.Next(0, 2) == 1)
                            {
                                orientation = Orientation.Horisontal;
                            }
                            else
                            {
                                orientation = Orientation.Vertical;
                            }
                        }
                        //shoot
                        if (orientation == Orientation.Horisontal)
                        {
                            try
                            {
                                if (map.Board[i][j + 1] == BoardSquareState.Empty)
                                {
                                    return GameBoard.Shoot(board, new[] {i, j + 1}, map);
                                }
                            }catch (Exception e)
                            {
                            }

                            try
                            {
                                if (map.Board[i][j - 1] == BoardSquareState.Empty)
                                {
                                    return GameBoard.Shoot(board, new[] {i, j - 1}, map);
                                }
                            }catch (Exception e)
                            {
                            }
                        }
                        if (orientation == Orientation.Vertical)
                        {
                            try
                            {
                                if (map.Board[i+1][j] == BoardSquareState.Empty)
                                {
                                    return GameBoard.Shoot(board, new[] {i+1, j}, map);
                                }
                            }catch (Exception e)
                            {
                            }

                            try
                            {
                                if (map.Board[i-1][j] == BoardSquareState.Empty)
                                {
                                    return GameBoard.Shoot(board, new[] {i-1, j}, map);
                                }
                            }catch (Exception e)
                            {
                            }
                        }
                    }
                }
            }
            return GameBoard.Shoot(board,randomCoords(map), map);
        }
        public static Player GetWinner(Player player1, Player player2)
        {
            if (player1.Board.Ships.Count == 0)
            {
                return player2;
            }

            if (player2.Board.Ships.Count == 0)
            {
                return player1;
            }
            return null;
        }
    }
}