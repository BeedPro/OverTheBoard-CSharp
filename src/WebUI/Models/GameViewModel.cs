using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Models
{
    public class GameViewModel
    {
        public string CurrentDisplayName { get; set; }
        public string OpponentDisplayName { get; set; }
        public string CurrentColour { get; set; }
        public string OpponentColour { get; set; }
        public int CurrentRating { get; set; }
        public int OpponentRating { get; set; }
    }
}
