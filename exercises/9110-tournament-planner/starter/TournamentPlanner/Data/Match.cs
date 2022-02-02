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
        public Player Player1 { get; set; } = new();       

        [Required]
        public Player Player2 { get; set; } = new();
       

        public Player Winner { get; set; } = new();
       
    }
}
