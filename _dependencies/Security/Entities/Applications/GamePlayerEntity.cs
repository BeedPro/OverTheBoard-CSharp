using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OverTheBoard.Data.Entities.Applications
{
    [Table("Players")]
    public class GamePlayerEntity
    {
        [Key]
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
        public string Colour { get; set; }
        public ChessGameEntity Game { get; set; }
        public string Pgn { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        
    }

}
