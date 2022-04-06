namespace OverTheBoard.Data.Entities
{
    public enum GameStatus
    {
        None,
        NotStarted,
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
        Level01 = 1,
        Level02 = 2,
        Level03 = 3,
        Level04 = 4,
        Level05 = 5,
        Level06 = 6,
        Level07 = 7,
        Level08 = 8,
        Level09 = 9,
        Level10 = 10
    }
}