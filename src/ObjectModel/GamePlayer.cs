using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace OverTheBoard.ObjectModel
{
    public class GamePlayer
    {
        public string ConnectionId { get; set; }
        public string Colour { get; set; }
        public string UserId { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public List<int> GameOutcome { get; set; }
        public List<GamePlayerEloOutcomes> EloOutcomes { get; set; }
        
    }
}