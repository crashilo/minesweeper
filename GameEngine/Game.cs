using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL;


namespace GameEngine
{
    public class Game
    {
        public CellState[,] Board { get; set; }

        private readonly AppDbContext _ctx;

        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int Bombs { get; set; }

        public int NotOpenedFields { get; set; }

        public Game(int boardHeight = 1, int boardWidth = 1)
        {
            if (boardHeight < 1 || boardWidth < 1)
            {
                throw new ArgumentException("Board size has to be at least 1x1!");
            }

            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            // initialize the board
            Board = new CellState[boardHeight, boardWidth];
        }

        public CellState[,] GetBoard()
        {
            var result = new CellState[BoardHeight, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }

        public void PlaceBomb(int sizeY, int sizeX, int bombs)
        {
            NotOpenedFields = BoardHeight * BoardWidth;
            Bombs = bombs;
            var count_bombs = 0;
            var bomb = new Random();
            while (count_bombs != bombs)
            {
                var xBomb = bomb.Next(0, sizeX);
                var yBomb = bomb.Next(0, sizeY);
                if (Board[yBomb, xBomb] != CellState.S)
                {
                    Board[yBomb, xBomb] = CellState.S;
                    count_bombs += 1;
                }
            }

            PlaceFieldValue();
        }

        public void PlaceFieldValue()
        {
            for (int yCor = 0; yCor < BoardHeight; yCor++)
            {
                for (int xCor = 0; xCor < BoardWidth; xCor++)
                {
                    if (GetValue(yCor, xCor) != CellState.S)
                    {
                        int bombsAround = 0;
                        for (int y = yCor - 1; y < yCor + 2; y++)
                        {
                            for (int x = xCor - 1; x < xCor + 2; x++)
                            {
                                try
                                {
                                    if (GetValue(y, x) == CellState.S)
                                    {
                                        bombsAround++;
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                            }
                        }

                        if (bombsAround == 0)
                        {
                            Board[yCor, xCor] = CellState.E;
                        }
                        else if (bombsAround == 1)
                        {
                            Board[yCor, xCor] = CellState.ONE;
                        }
                        else if (bombsAround == 2)
                        {
                            Board[yCor, xCor] = CellState.TWO;
                        }
                        else if (bombsAround == 3)
                        {
                            Board[yCor, xCor] = CellState.THREE;
                        }
                        else if (bombsAround == 4)
                        {
                            Board[yCor, xCor] = CellState.FOUR;
                        }
                        else if (bombsAround == 5)
                        {
                            Board[yCor, xCor] = CellState.FIVE;
                        }
                        else if (bombsAround == 6)
                        {
                            Board[yCor, xCor] = CellState.SIX;
                        }
                        else if (bombsAround == 7)
                        {
                            Board[yCor, xCor] = CellState.SEVEN;
                        }
                        else if (bombsAround == 8)
                        {
                            Board[yCor, xCor] = CellState.EIGHT;
                        }
                    }
                }
            }
        }

        public void Move(int posY, int posX)
        {
            if (Board[posY, posX] == CellState.S)
            {
                Board[posY, posX] = CellState.B;
            }
            else if (Board[posY, posX] == CellState.E)
            {
                Board[posY, posX] = CellState.X;
                NotOpenedFields--;
                OpenAround(posY, posX);
            }
            else if (Board[posY, posX] == CellState.ONE)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.ONEOPEN;
            }
            else if (Board[posY, posX] == CellState.TWO)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.TWOOPEN;
            }
            else if (Board[posY, posX] == CellState.THREE)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.THREEOPEN;
            }
            else if (Board[posY, posX] == CellState.FOUR)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.FOUROPEN;
            }
            else if (Board[posY, posX] == CellState.FIVE)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.FIVEOPEN;
            }
            else if (Board[posY, posX] == CellState.SIX)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.SIXOPEN;
            }
            else if (Board[posY, posX] == CellState.SEVEN)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.SEVENOPEN;
            }
            else if (Board[posY, posX] == CellState.EIGHT)
            {
                NotOpenedFields--;
                Board[posY, posX] = CellState.EIGHTOPEN;
            }
        }

        public void OpenAround(int i, int j)
        {
            for (int y = i - 1; y < i + 2; y++)
            {
                for (int x = j - 1; x < j + 2; x++)
                {
                    try
                    {
                        Move(y, x);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                    }
                }
            }
        }

        public CellState GetValue(int y, int x)
        {
            return Board[y, x];
        }
    }
}