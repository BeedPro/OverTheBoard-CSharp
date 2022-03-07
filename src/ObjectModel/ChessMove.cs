namespace OverTheBoard.ObjectModel
{
    public class ChessMove
    {
        public string GameId { get; set; }
        public string Fen { get; set; }
        public string Pgn { get; set; }
        public string Orientation { get; set; }
        public int whiteRemaining { get; set; }
        public int blackRemaining { get; set; }
    }
}