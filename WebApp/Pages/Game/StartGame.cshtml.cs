using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DAL;
using Domain;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class StartGame : PageModel
    {
        private readonly AppDbContext _context;

        public StartGame(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public string? FieldSize { get; set; }
        [BindProperty] public GameOptions GameOptions { get; set; } = new GameOptions();
        public string SaveName { get; set; } = default!;
        public int GameId { get; set; }

        public bool isCorrect { get; set; } = true;

        public bool BombAmount { get; set; } = true;
        
        public async Task OnGet(bool? correct, bool? bombAmount ,string? saveName,int? gameId)
        {
            if (gameId != null)
            {
                GameId = gameId.Value;
            }

            if (bombAmount != null)
            {
                BombAmount = bombAmount.Value;
            }
            if (correct != null)
            {
                isCorrect = correct.Value;
            }
            if (saveName != null)
            {
                SaveName = saveName;
            }
            else
            {
                SaveName = "SaveName";
            }

//            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (FieldSize == "Back to Main Menu")
            {
                return RedirectToPage("./MainMenu");
                    
            }

            if (ModelState.IsValid)
            {
                if (FieldSize == "Small Field")
                {
                    GameOptions.Height = 9;
                    GameOptions.Width = 9;
                    GameOptions.Bombs = 10;
                }

                if (FieldSize == "Medium Field")
                {
                    GameOptions.Height = 16;
                    GameOptions.Width = 16;
                    GameOptions.Bombs = 40;
                }

                if (FieldSize == "Large Field")
                {
                    GameOptions.Height = 16;
                    GameOptions.Width = 30;
                    GameOptions.Bombs = 99;
                }

                if (GameOptions.Bombs > GameOptions.Height * GameOptions.Width - 1)
                {
                    BombAmount = false;
                }

                foreach (GameSettings settings in _context.GameSettingses)
                {
                    if (settings.GameName == GameOptions.GameName)
                    {
                        isCorrect = false;
                    }
                }

                if (!isCorrect || !BombAmount)
                {
                    return RedirectToPage("./StartGame", new {correct = isCorrect, bombAmount = BombAmount});
                }

                var game = new GameEngine.Game(GameOptions.Height, GameOptions.Width);
                game.PlaceBomb(GameOptions.Height, GameOptions.Width, GameOptions.Bombs);
                string gameCells = "";
                foreach (CellState variable in game.GetBoard())
                {
                    gameCells += Convert.ToInt32(variable) + ",";
                }

                gameCells = gameCells.Remove(gameCells.Length - 1);

                var gameState = new GameSettings()
                {
                    BoardWidth = GameOptions.Width,
                    BoardHeight = GameOptions.Height,
                    GameName = GameOptions.GameName,
                    BombAmount = GameOptions.Bombs,
                    GameState = gameCells
                };
                _context.GameSettingses.Add(gameState);
                await _context.SaveChangesAsync();
                return RedirectToPage("./PlayGame", new {gameId = gameState.GameId});
            }

            return Page();
        }
    }

    public class GameOptions
    {
        public int Width { get; set; } = 3;
        public int Height { get; set; } = 3;
        public int Bombs { get; set; } = 3;
        [MinLength(2)] [MaxLength(32)] public string GameName { get; set; } = "SaveName";
    }
}