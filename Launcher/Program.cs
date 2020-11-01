using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using ConsoleUI;
using GameEngine;
using MenuSystem;

namespace Launcher
{
    class Program
    {
        public class ConsoleSpiner
        {
            int counter;
            string cursor = "<-";
            public ConsoleSpiner()
            {
                counter = 0;
                Console.CursorVisible = false;
            }

            public void Turn()
            {
                counter++;
                if (counter == 7)
                {
                    counter = 0;
                }
                Thread.Sleep(100);
                switch (counter % 6)
                {
                    case 1:
                        Console.Write("{0} ", cursor);
                        Console.SetCursorPosition(CurrentLenght+( 0), CurrentPos);
                        break;
                    case 2:
                        Console.Write(" {0}", cursor);
                        Console.SetCursorPosition(CurrentLenght+( 0), CurrentPos);
                        break;
                    case 3:
                        Console.Write("  {0}", cursor);
                        Console.SetCursorPosition(CurrentLenght+( 1), CurrentPos);
                        break;
                    case 4:
                        Console.Write("   {0}", cursor);
                        Console.SetCursorPosition(CurrentLenght+( 2), CurrentPos);
                        break;
                    case 5:
                        Console.Write("  {0} ", cursor);
                        Console.SetCursorPosition(CurrentLenght+( 2), CurrentPos);
                        break;
                    case 0:
                        Console.Write(" {0} ", cursor);
                        Console.SetCursorPosition(CurrentLenght+( 1), CurrentPos);
                        break;
                }
                // Console.WriteLine(Console.CursorLeft);


            }
        }
        public static int CurrentPos;
        public static int CurrentLenght;

        private static void Main(string[] args)
        {
            var gameMenu = new Menu(1)
            {
                Title = "Start a nw game of Minesweeper",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Small field",
                            CommandToExecute = MakeSmallField
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Title = "Medium field",
                            CommandToExecute = MakeMediumField
                        }
                    },
                    {
                        "3", new MenuItem()
                        {
                            Title = "Large field",
                            CommandToExecute = MakeLargeField
                        }
                    },
                    {
                        "4", new MenuItem()
                        {
                            Title = "Custom field",
                            CommandToExecute = MakeCustomField
                        }
                    },
                }
            };
            
            var menu0 = new Menu
            {
                Title = "Minesweeper Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Title = "Load Game",
                            CommandToExecute = ContinueGame
                        }
                    },
                }
            };
            menu0.Run();
            Console.WriteLine("end");
            Thread.Sleep(1000);
        }

        private static string MakeSmallField()
        {
            return CreateGame(9, 9, 10);
        }

        private static string MakeMediumField()
        {
            return CreateGame(16, 16, 40);
        }

        private static string MakeLargeField()
        {
            return CreateGame(16, 30, 99);
        }

        private static string MakeCustomField()
        {
            string y = default!;
            string x;
            string bombs;
            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter field HEIGHT -> ");
                y = Console.ReadLine();
                try
                {
                    int.Parse(y);
                    correct = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Height must be filled with number!");
                    correct = false;
                }
            } while (!correct);

            correct = false;

            do
            {
                Console.WriteLine("Enter field WIDTH -> ");
                x = Console.ReadLine();
                try
                {
                    int.Parse(x);
                    correct = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Width must be filled with number!");
                    correct = true;
                }
            } while (!correct);

            correct = false;

            do
            {
                Console.WriteLine("Enter amount of BOMBS -> ");
                bombs = Console.ReadLine();
                try
                {
                    int.Parse(bombs);
                    correct = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Amount of bombs must be filled with number!");
                    correct = true;
                }
            } while (!correct);

            return CreateGame(int.Parse(y), int.Parse(x), int.Parse(bombs));
        }

        private static (int userXInput, int userYInput, bool exit) SetCoordinates(Game game, string inputOption, int x,
            int y, int userXInt, int userYInt, bool correctInput)
        {
            Console.Clear();
            bool exit = false;
            bool userCanceled = false;
            if (inputOption.Equals("x"))
            {
                GameUI.PrintBoard(game);
                (userXInt, userCanceled, exit, correctInput) = GetUserIntInput("X", 1, x,
                    0, "", "S", game, "X", correctInput);
                if (exit)
                {
                    return (userXInt, userYInt, exit);
                }


                if (userXInt == 0 || !correctInput)
                {
                    return SetCoordinates(game, "x", x, y, userXInt, userYInt, correctInput);
                }

                return SetCoordinates(game, "y", x, y, userXInt, userYInt, correctInput);
            }

            if (inputOption.Equals("y"))
            {
                GameUI.PrintBoard(game);
                (userYInt, userCanceled, exit, correctInput) = GetUserIntInput("Y", 1, y,
                    0, "", "S", game, "X", correctInput);

                if (exit)
                {
                    return (userXInt, userYInt, exit);
                }


                if (userCanceled)
                {
                    return SetCoordinates(game, "x", x, y, userXInt, userYInt, correctInput);
                }

                if (userYInt == 0 || !correctInput)
                {
                    return SetCoordinates(game, "y", x, y, userXInt, userYInt, correctInput);
                }
            }

            return (userXInt, userYInt, exit);
        }

        private static void PlayGame(Game game, int y, int x)
        {
            Console.Clear();
            var userXint = 0;
            var userYint = 0;
            var done = false;
            do
            {
                Console.Clear();
                (userXint, userYint, done) = SetCoordinates(game, "x", x, y, userXint, userYint, true);

                if (!done)
                {
                    game.Move(userYint - 1, userXint - 1);
                    if (game.GetValue(userYint - 1, userXint - 1) == CellState.B)
                    {
                        Console.WriteLine("OOPS BOMB!!!");
                        Fonts.gameOver();
                        done = true;
                    }

                    if (game.Bombs == game.NotOpenedFields)
                    {
                        Fonts.gamewWon();
                        done = true;
                    }
                }
            } while (!done);
        }

        private static string CreateGame(int y, int x, int bombs)
        {
            var game = new Game(y, x);
            game.PlaceBomb(y, x, bombs);
            PlayGame(game, y, x);
            Console.WriteLine("Hit ENTER to return to Main Menu");
            Console.Read();
            return "";
        }

        private static string ContinueGame()
        {
            int width;
            int height;
            string gameSates;

            (width, height, gameSates) = LoadGame.LoadSettings();
            String[] gameStateList = gameSates.Split(",");

            var game = new Game(height, width);
            game.GetBoard();

            int indexOfList = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    game.Board[i, j] = (CellState) int.Parse(gameStateList[indexOfList]);
                    indexOfList++;
                }
            }

            PlayGame(game, height, width);
            Console.WriteLine("Hit ENTER to return to Main Menu");
            Console.Read();
            return "";
        }

        private static (int result, bool isCanceled, bool exit, bool isCorrecr) GetUserIntInput(string prompt, int min,
            int max,
            int? cancelIntValue,
            string cancelStrValue, string saveValue, Game game, string exitValue, bool isCorrect)
        {
            do
            {
                Console.WriteLine($"Enter {prompt} coordinate");
                Console.WriteLine($"To CANCEL input enter: {cancelIntValue}");
                Console.WriteLine($"To SAVE enter: {saveValue}");
                Console.WriteLine($"To EXIT enter: {exitValue}");
                if (!isCorrect && prompt.Equals("X"))
                {
                    Console.WriteLine($"{prompt} coordinates must be between 1 and {game.BoardWidth}!");
                }
                else if (!isCorrect && prompt.Equals("Y"))
                {
                    Console.WriteLine($"{prompt} coordinates must be between 1 and {game.BoardHeight}!");
                }

                Console.Write(">");
                var consoleLine = Console.ReadLine();
                if (consoleLine.ToUpper() == saveValue)
                {
                    SaveGame.SaveSettings(game);
                    GameUI.PrintBoard(game);
                }

                if (consoleLine.ToUpper() == exitValue)
                {
                    return (0, false, true, true);
                }

                if (consoleLine == cancelStrValue) return (0, true, false, true);

                if (int.TryParse(consoleLine, out var userInt))
                {
                    if (prompt.Equals("X") && (userInt > game.BoardWidth || userInt < 0))
                    {
                        return (0, false, false, false);
                    }

                    if (prompt.Equals("Y") && (userInt > game.BoardHeight || userInt < 0))
                    {
                        return (0, false, false, false);
                    }

                    return userInt == cancelIntValue ? (userInt, true, false, true) : (userInt, false, false, true);
                }

                if (consoleLine.ToUpper() != saveValue || consoleLine.ToUpper() != exitValue)
                    Console.WriteLine($"'{consoleLine}' cant be converted to int value!");
                Console.Clear();
            } while (true);
        }
    }
}