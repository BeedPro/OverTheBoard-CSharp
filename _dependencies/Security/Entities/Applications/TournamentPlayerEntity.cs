using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OverTheBoard.Data.Entities.Applications
{
    [Table("TournamentPlayers")]
    public class TournamentPlayerEntity
    {
        [Key]
        public int TournamentPlayerId { get; set; }
        public int TournamentId { get; set; }
        public Guid UserId { get; set; }
        public TournamentEntity Tournament { get; set; }
    }

}
