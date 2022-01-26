using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Data
{
    public class Match
    {
        public int ID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Round { get; set; }

        [Required]
        public List<Player> Players { get; set; } = new();

        public Player Winner { get; set; }
    }
}
