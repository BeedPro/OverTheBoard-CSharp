using System;

namespace OverTheBoard.ObjectModel.Queues
{

    public class GameQueueItem
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }


    public class UnrankedGameQueueItem : GameQueueItem
    {
        public int Rating { get; set; }
    }
    
    public class TournamentQueueItem : GameQueueItem
    {
        public Guid Identifier { get; set; }
        public int Level { get; set; }
    }
    
    
    public class GameCompletionQueueItem
    {
        public string GameId { get; set; }
        public int Level { get; set; }
    }
}