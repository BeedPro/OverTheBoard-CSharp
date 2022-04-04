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
        public ICollection<TournamentUserEntity> Players { get; set; }
    }

    [Table("TournamentUsers")]
    public class TournamentUserEntity
    {
        [Key]
        public int TournamentUserId { get; set; }
        public int TournamentId { get; set; }
        public Guid UserId { get; set; }
        public TournamentEntity Tournament { get; set; }
    }

}
