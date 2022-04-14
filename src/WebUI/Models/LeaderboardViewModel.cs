using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Models
{
    public class LeaderboardViewModel
    {
        public List<KeyValuePair<string, string>> YourDetails { get; set; }
        public List<GamePlayerStatsModel> TopRanks { get; set; }
    }
}
