using System;
using System.Collections.Generic;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Entities.Applications;

namespace OverTheBoard.ObjectModel
{
    public class ChessGame
    {
        public ChessGame()
        {
            Players = new List<GamePlayer>();
        }

        public string Identifier { get; set; }
        public List<GamePlayer> Players { get; set; }
        public string Fen { get; set; }
        public string Pgn { get; set; }
        public DateTime? LastMoveAt { get; set; }
        public string NextMoveColour { get; set; }
        public GameStatus Status { get; set; }
        public GameType Type { get; set; }
    }
}