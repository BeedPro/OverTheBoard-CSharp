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
        public string Colour { get; set; }
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

    }

    public class GamePlayer
    {
        public string ConnectionId { get; set; }
        public string Colour { get; set; }
        public string UserId { get; set; }
    }

    public enum GameType
    {
        None,
        Unranked,
        Brackets
    }

}
