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

        public static bool CheckPlace(GameBoard Board, int row,int column,int shipLen,bool rotation)
        {
            for (int i = 0; i < shipLen; i++) //checking if the ship itself will fit inside the area
            {
                if (rotation == true)
                {
                    try
                    {
                        if (Board.Board[row + i][column] == BoardSquareState.Ship)
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
                        if (Board.Board[row][column + i] == BoardSquareState.Ship)
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
                                if (Board.Board[row + i - 1][column + j - 1] == BoardSquareState.Ship)
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
                                if (Board.Board[row + j - 1][column + i - 1] == BoardSquareState.Ship)
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
        
        public static bool AIPlacing(List<int> shipsList,GameBoard Board){
            bool rotation = false;
            Random rnd = new Random();
            var shipsAmount = shipsList[0] + shipsList[1] + shipsList[2] + shipsList[3];
            var shipLen = 4;
            var tries = 0;
            var row = 0;
            var column = 0;
            while (shipsAmount>0)
            {
                if (shipLen == 4 && shipsList[0] == 0 || shipLen == 3 && shipsList[1] == 0
                                                      || shipLen == 2 && shipsList[2] == 0 || shipLen == 1 && shipsList[3] == 0)
                {
                    shipLen--;
                    continue;
                }
                bool available = AI.CheckPlace(Board,row,column,shipLen,rotation);
    
                if (available == true)
                {
                    SetPlace(Board,row,column,shipLen,rotation);
                    if(shipLen == 1){shipsList[3]--;}if(shipLen == 2){shipsList[2]--;}
                    if(shipLen==3){shipsList[1]--;}if(shipLen==4){shipsList[0]--;}
                }
                row = row + 2;
                if (tries == 5)
                {
                    column = column + 5;
                    row = 0;
                }
                tries++;
                shipsAmount = shipsList[0] + shipsList[1] + shipsList[2] + shipsList[3];
                if (tries > 50)
                {
                    return false;
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
        
        public static void SetPlace(GameBoard board, int row,int column,int shipLen,bool rotation)
        {
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
            for (int i = 0; i < map.Board.Count; i++)
            {
                for (int j = 0; j < map.Board[i].Count; j++)
                {
                    if (map.Board[i][j] == BoardSquareState.Hit)
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
                            if (rnd.Next(0, 1) == 1)
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
    }
}