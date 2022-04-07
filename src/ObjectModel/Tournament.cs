using System;
using System.Collections.Generic;

namespace OverTheBoard.ObjectModel
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Identifier { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TournamentPlayer> Players { get; set; }
    }
}