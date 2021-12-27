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

    public class ChessGame
    {
        public List<GamePlayer> Players { get; set; }
    }

    public class GamePlayer
    {
        public string ConnectionId { get; set; }
        public string Colour { get; set; }
        public string UserId { get; set; }
        public string GameId { get; set; }
    }

    public enum GameType
    {
        None,
        Unranked,
        Brackets
    }

}
