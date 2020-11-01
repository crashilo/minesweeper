using System;
using System.Collections.Generic;
using System.Text.Json;
using GameEngine;

namespace JsonSettings
{
    public class JsonMethods
    {
        public GameSettings InsertInfo(string name ,int boardWidth, int boardHeight, List<CellState> list)
        {
            var gamesetting = new GameSettings()
            {
                GameName = name,
                BoardWidth = boardWidth,
                BoardHeight = boardHeight,
                CellStatee = list
            };
            return gamesetting;
        }
        public GameSettings LoadSettings(string fileName)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);
            var res = JsonSerializer.Deserialize<GameSettings>(jsonString);

            return res;
        }



        public void SaveSettings(GameSettings gameSettings, string fileName)
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize<GameSettings>(gameSettings, jsonOptions);
            using (var writer = System.IO.File.CreateText(fileName))
            {
                writer.Write(jsonString);
                // writer gets closed here - writer.Dispose() is called
            }


        }
        

    }
    
    public class GameSettings
    {
        public string GameName { get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }

        public List<CellState> CellStatee { get; set; }
    }
}