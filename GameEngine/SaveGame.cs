using System;
using System.Linq;
using DAL;
using Domain;

namespace GameEngine
{
    public class SaveGame
    {
        public static void SaveSettings(Game game)
        {
            using (var ctx = new AppDbContext())
            {
                string saveName;
                string gameState = "";
                foreach (CellState variable in game.GetBoard())
                {
                    gameState += Convert.ToInt32(variable)+",";
                }

                gameState = gameState.Remove(gameState.Length - 1);

                Console.WriteLine("Enter save name");
                do
                {
                    saveName = Console.ReadLine();
                } while (saveName == "");


                GameSettings data = new GameSettings();
                var dataFromDBase = ctx.GameSettingses.ToList();
                foreach (GameSettings gameSettings in dataFromDBase)
                {
                    if (saveName.Equals(gameSettings.GameName))
                    {
                        gameSettings.GameName = saveName;
                        gameSettings.BoardHeight = game.BoardHeight;
                        gameSettings.BoardWidth = game.BoardWidth;
                        gameSettings.GameState = gameState;
                        ctx.SaveChanges();
                        return;
                    }
                }

                data.GameName = saveName;
                data.BoardHeight = game.BoardHeight;
                data.BoardWidth = game.BoardWidth;
                data.GameState = gameState;
                data.BombAmount = game.Bombs;
                ctx.GameSettingses.Add(data);
                ctx.SaveChanges();
            }
        }
    }
}