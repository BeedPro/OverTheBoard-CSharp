using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OverTheBoard.Data.Entities.Applications
{
    [Table("Games")]
    public class ChessGameEntity
    {
        [Key]
        public int GameId { get; set; }

        public Guid Identifier { get; set; }

        public string Fen { get; set; }
        public DateTime StartTime { get; set; }

        [ForeignKey("GameId")]
        public ICollection<GamePlayerEntity> Players { get; set; }

        public int Period { get; set; } //in minutes
    }

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
        public TimeSpan TimeRemain { get; set; }
        public DateTime LastMoveAt { get; set; }
    }

}
