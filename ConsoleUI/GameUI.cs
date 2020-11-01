using System;
using System.ComponentModel;
using GameEngine;

namespace ConsoleUI
{
    public static class GameUI
    {
        private static readonly string _verticalSeparator = "|";
        private static readonly string _horizontalSeparator = "-";
        private static readonly string _centerSeparator = "+";

        public static void PrintBoard(Game game)
        {
            var board = game.GetBoard();
            var upperDigitLine = "   ";
//            string bottomDigitLine ="";
//            int count = 3;
            int cellInt = 1;
            string fieldBorder = "=";
            for (int i = 1; i <= game.BoardWidth; i++)
            {
                if (i != game.BoardWidth)
                {
                    fieldBorder += "-----+";
                }
                 
                if (cellInt > 9)
                {
                    upperDigitLine += $"{cellInt.ToString()}    ";
                    cellInt++;
                }
                else
                {
                    upperDigitLine += $"{cellInt.ToString()}     ";
                    cellInt++;
                }
                
            }

            fieldBorder += "-----=";

            Console.WriteLine(upperDigitLine);
            Console.WriteLine(fieldBorder);
            
            for (int yIndex = 0; yIndex < game.BoardHeight; yIndex++)
            {
                var line = $"|";
                for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                {
                    line += $"  {GetSingleState(board[yIndex, xIndex])}";
                    if (xIndex < game.BoardWidth - 1)
                    {
                        line += $"  {_verticalSeparator}";
                    }

                    if (xIndex == game.BoardWidth - 1)
                        line += $"  {_verticalSeparator}{(yIndex + 1)}";
                }

                Console.WriteLine(line);

                if (yIndex < game.BoardHeight - 1)
                {
                    line = "|";
                    for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                    {
                        line += $"{_horizontalSeparator} {_horizontalSeparator} {_horizontalSeparator}";
                        if (xIndex < game.BoardWidth - 1)
                        {
                            line += _centerSeparator;
                        }

                        if (xIndex == game.BoardWidth - 1)
                            line += _verticalSeparator;
                    }

                    Console.WriteLine(line);
                }
            }

            Console.WriteLine(fieldBorder);
        }

        public static string GetSingleState(CellState state)
        {
            switch (state)
            {
                case CellState.E:
                    return " ";
                case CellState.X:
                    return "0";
                case CellState.S:
                    return " ";
                case CellState.B:
                    return "B";
                case CellState.ONE:
                    return " ";
                case CellState.ONEOPEN:
                    return "1";
                case CellState.TWO:
                    return " ";
                case CellState.TWOOPEN:
                    return "2";
                case CellState.THREE:
                    return " ";
                case CellState.THREEOPEN:
                    return "3";
                case CellState.FOUR:
                    return " ";
                case CellState.FOUROPEN:
                    return "4";
                case CellState.FIVE:
                    return " ";
                case CellState.FIVEOPEN:
                    return "5";
                case CellState.SIX:
                    return " ";
                case CellState.SIXOPEN:
                    return "6";
                case CellState.SEVEN:
                    return " ";
                case CellState.SEVENOPEN:
                    return "7";
                case CellState.EIGHT:
                    return " ";
                case CellState.EIGHTOPEN:
                    return "8";
                default:
                    throw new InvalidEnumArgumentException("Unknown enum option! " + state);
            }
        }
    }
}