namespace OverTheBoard.Data.Entities
{
    public enum GameStatus
    {
        None,
        InProgress,
        Completed
    }
    
    public enum GameType
    {
        None,
        Unranked,
        Ranked
    }

    public enum UserRank
    {
        None = 0,
        Unranked = 5,
        Level4 = 4,
        Level3 = 3,
        Level2 = 2,
        Level1 = 1
    }
}