using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game
{
    public class PlayGame : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public PlayGame(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<GameSettings> GameSettings { get; set; } = default!;
        public int GameId { get; set; }
        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }
        public CellState[,] Board { get; set; } = default!;
        public bool GameOver { get; set; }

        public bool WinGame { get; set; }

        public CellState[] NotOpenValues =
        {
            CellState.E, CellState.S, CellState.ONE, CellState.TWO, CellState.THREE,
            CellState.FOUR, CellState.FIVE, CellState.SIX, CellState.SEVEN, CellState.EIGHT
        };


        public async Task<ActionResult> OnGet(int? gameId, int? col, int? row, bool? over, bool? win)
        {
            if (win != null)
            {
                WinGame = win.Value;
            }

            if (over != null)
            {
                GameOver = over.Value;
            }

            GameSettings = await _context.GameSettingses.ToListAsync();
            if (gameId == null)
            {
                return RedirectToPage("./StartGame");
            }
            
            var match = false;
            foreach (GameSettings settings in _context.GameSettingses)
            {
                if (settings.GameId == gameId)
                {
                    match = true;
                }
            }

            if (!match)
            {
                return RedirectToPage("./MainMenu");
            }

            GameId = gameId.Value;
            BoardHeight = GameSettings.First(a => a.GameId == gameId).BoardHeight;
            BoardWidth = GameSettings.First(a => a.GameId == gameId).BoardWidth;
            var game = new GameEngine.Game(GameSettings.First(a => a.GameId == gameId).BoardHeight,
                GameSettings.First(a => a.GameId == gameId).BoardWidth);
            var indexOfList = 0;
            var gameCells = GameSettings.First(a => a.GameId == gameId)
                .GameState.ToString();
            var gameCallsList = gameCells.Split(",");
            for (var i = 0; i < GameSettings.First(a => a.GameId == gameId).BoardHeight; i++)
            {
                for (var j = 0; j < GameSettings.First(a => a.GameId == gameId).BoardWidth; j++)
                {
                    game.Board[i, j] = (CellState) int.Parse(gameCallsList[indexOfList]);
                    indexOfList++;
                }
            }

            Board = game.Board;
            if (col != null && row != null)
            {
                game.Move(row.Value, col.Value);
                if (game.GetValue(row.Value, col.Value) == CellState.B)
                {
                    return RedirectToPage("./PlayGame", new {over = true, gameId = GameId});
                }

                var emptyCellCount = 0;
                foreach (CellState cellState in Board)
                {
                    if (NotOpenValues.Contains(cellState))
                    {
                        emptyCellCount++;
                    }
                }


                string gameState = "";
                foreach (CellState variable in Board)
                {
                    gameState += Convert.ToInt32(variable) + ",";
                }

                gameState = gameState.Remove(gameState.Length - 1);

                _context.GameSettingses.First(a => a.GameId == gameId).GameState = gameState;
                await _context.SaveChangesAsync();
                if (emptyCellCount == GameSettings.First(a => a.GameId == gameId).BombAmount)
                {
                    return RedirectToPage("./PlayGame", new {win = true, gameId = GameId});
                }
            }

            return Page();
        }
        public async Task<ActionResult> DeleteGame(int? gameId)
        {
            _context.GameSettingses.Remove(GameSettings.First(a => a.GameId == gameId));
            await _context.SaveChangesAsync();
            return RedirectToPage("./MainMenu");
        }
    }
    
}