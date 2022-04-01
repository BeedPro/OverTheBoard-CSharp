namespace OverTheBoard.ObjectModel.Queues
{
    public class TournamentQueueItem : GameQueueItem
    {
        public int TournamentQueueId { get; set; }
        public int Level { get; set; }
    }
}