using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using GameEngine;

namespace WebApp.Pages.Game
{
    public class MainMenu : PageModel
    {
        private readonly AppDbContext _context;
        [BindProperty] public int GameId { get; set; } = 0;

        public MainMenu(AppDbContext context)
        {
            _context = context;
        }

        public IList<GameSettings> GameSettings { get; set; } = default!;
        public string? CollapseShow { get; set; }

        public async Task<ActionResult> OnGet(int? gameId, string collapse)
        {
            if (collapse != null)
            {
                CollapseShow = collapse;
            }
            else
            {
                CollapseShow = "";
            }
            if (gameId != null)
            {
                _context.GameSettingses.Remove(_context.GameSettingses.First(a => a.GameId == gameId));
                _context.SaveChanges();
            }
            GameSettings = await _context.GameSettingses.ToListAsync();
            return Page();
        }
    }
}