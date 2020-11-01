using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameSettings
    {
        [Key] public int GameId { get; set; }

        public string GameName { get; set; } = default!;
        public int BoardWidth { get; set; } = default!;
        public int BoardHeight { get; set; } = default!;
        public string GameState { get; set; } = default!;

        public int BombAmount { get; set; } = default!;
    }
}