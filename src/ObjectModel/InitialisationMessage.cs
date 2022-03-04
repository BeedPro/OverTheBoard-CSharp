using OverTheBoard.Data.Entities.Applications;
using System;
using System.Collections.Generic;

namespace OverTheBoard.ObjectModel
{
    public class InitialisationMessage
    {
            public string UserId { get; set; }
            public string GameId { get; set; }
            public string ConnectionId { get; set; }
            public string Type { get; set; }
    }

    public class ChessMove
    {
        public string GameId { get; set; }
        public string Fen { get; set; }
        public string Pgn { get; set; }
        public string Orientation { get; set; }
        public int whiteRemaining { get; set; }
        public int blackRemaining { get; set; }
    }


    public class UnrankedGameQueueItem
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }

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
    }

    public class GamePlayer
    {
        public string ConnectionId { get; set; }
        public string Colour { get; set; }
        public string UserId { get; set; }
        public TimeSpan TimeRemaining { get; set; }
    }

    public class GameInfo
    {
        public string Identifier { get; set; }
        public string  WhiteUser { get; set; }
        public string BlackUser { get; set; }
        public GameStatus Status { get; set; }
    }

    public enum GameType
    {
        None,
        Unranked,
        Brackets
    }

}
