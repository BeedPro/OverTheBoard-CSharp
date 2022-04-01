using System;
using System.Collections.Generic;

namespace OverTheBoard.ObjectModel.Queues
{
    public class DivisionItem
    {
        public int RoundNumber { get; set; }
        public DateTime DateTime { get; set; }
        public List<GameQueueItem> GameQueueItems { get; set; }

    }
}