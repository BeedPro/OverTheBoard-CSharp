using System;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Entities.Applications;

namespace OverTheBoard.ObjectModel
{
    public class GameInfo
    {
        public string Identifier { get; set; }
        public string  WhiteUser { get; set; }
        public string BlackUser { get; set; }
        public GameStatus Status { get; set; }
        public DateTime StartTime { get; set; }
    }
}