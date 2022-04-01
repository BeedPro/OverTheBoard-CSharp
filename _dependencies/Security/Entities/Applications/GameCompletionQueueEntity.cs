using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OverTheBoard.Data.Entities.Applications
{
    [Table("CompletionQueue")]
    public class GameCompletionQueueEntity
    {
        [Key] 
        public int CompletionQueueId { get; set; }
        public string GameId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Level { get; set; }
    } 
    
    [Table("TournamentQueue")]
    public class TournamentQueueEntity
    {
        [Key] 
        public int TournamentQueueId { get; set; }
        public Guid UserId { get; set; }
        //public Guid Identifier { get; set; }
        public int Level { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    //[Table("Rankings")]
    //public class UserRankingEntity
    //{
    //    [Key]
    //    public int RankingId { get; set; }
    //    public GameType Type { get; set; }
    //    public Guid UserId { get; set; }
    //    public int Score { get; set; }
    //    public int Win { get; set; }
    //    public int Lose { get; set; }
    //    public int Draw { get; set; }
    //}
}