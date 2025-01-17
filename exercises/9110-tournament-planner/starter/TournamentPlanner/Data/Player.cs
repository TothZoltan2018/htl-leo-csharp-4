﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Data
{
    public class Player
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        
        //public Match Match { get; set; }

        //public int MatchId { get; set; }

        //public List<Match> Matches { get; set; } = new();
    }
}
