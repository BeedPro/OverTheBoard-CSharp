using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverTheBoard.ObjectModel
{
    public class GameOverStatus
    {
        public string ConnectionId { get; set; }
        public string GameId { get; set; }
        public EloOutcomesType WhiteOutcome { get; set; }
        public EloOutcomesType BlackOutcome { get; set; }
    }
}
