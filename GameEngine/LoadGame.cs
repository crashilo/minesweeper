using System;
using System.Linq;
using DAL;
using Domain;

namespace GameEngine
{
    public class LoadGame
    {
        public static (int width, int height, string gameState) LoadSettings()
        {
            using (var ctx = new AppDbContext())
            {
                var dataFromDBase = ctx.GameSettingses.ToList();
                string saveName;

                Console.Clear();
                Console.WriteLine("Available saves:");
                foreach (GameSettings gameData in dataFromDBase)
                {
                    Console.WriteLine(gameData.GameName);
                }

                bool isInBase = false;
                Console.WriteLine("Enter saved game name:");
                do
                {
                    Console.Clear();
                    Console.WriteLine("Available saves:");
                    foreach (GameSettings gameData in dataFromDBase)
                    {
                        Console.WriteLine(gameData.GameName);
                    }

                    Console.WriteLine("Enter saved game name:");
                    saveName = Console.ReadLine();
                    foreach (GameSettings gameData in dataFromDBase)
                    {
                        if (saveName == gameData.GameName)
                        {
                            isInBase = true;
                            break;
                        }
                    }
                } while (!isInBase);


                var height = ctx.GameSettingses.Where(x => x.GameName == (saveName)).Select(x => x.BoardHeight)
                    .First();
                var width = ctx.GameSettingses.Where(x => x.GameName == (saveName)).Select(x => x.BoardWidth)
                    .First();
                var gameState = ctx.GameSettingses.Where(x => x.GameName == (saveName)).Select(x => x.GameState)
                    .First();
                return (width, height, gameState);
            }
        }
    }
}