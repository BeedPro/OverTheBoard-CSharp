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
    [Table("TournamentUsers")]
    public class TournamentUserEntity
    {
        [Key]
        public int TournamentUserId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid UserId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
