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
        public int Period { get; set; } //in minutes
        public DateTime? LastMoveAt { get; set; }
        [ForeignKey("GameId")]
        public ICollection<GamePlayerEntity> Players { get; set; }
        public string NextMoveColour { get; set; }
        public GameStatus Status { get; set; }
    }

    public struct GameOutcome
    {
        public const double Win = 1.0;
        public const double Draw = 0.5;
        public const double Lose = 0.0;
    }
    public enum GameStatus
    {
        None,
        InProgress,
        Completed
    }
}
