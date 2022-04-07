using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OverTheBoard.Data.Entities.Applications
{
    [Table("Tournaments")]
    public class TournamentEntity
    {
        [Key]
        public int TournamentId { get; set; }
        public Guid TournamentIdentifier { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("TournamentId")]
        public ICollection<TournamentPlayerEntity> Players { get; set; }
    }
}