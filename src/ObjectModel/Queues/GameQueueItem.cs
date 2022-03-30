namespace OverTheBoard.ObjectModel.Queues
{
    public class GameQueueItem
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public int Rating { get; set; }
    }

    public class GameCompletionQueueItem
    {
        public string GameId { get; set; }
    }
}