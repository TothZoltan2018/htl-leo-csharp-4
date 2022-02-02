using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Data;

namespace TournamentPlanner.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TournamentController : Controller
    {
        TournamentPlannerDbContext context;
        public TournamentController(TournamentPlannerDbContext context)
        {
            this.context = context;
        }

        [Route("players")]
        [HttpGet]
        public async Task<IEnumerable<Player>> GetPlayers(string name = null) // [FromQuery], from this uri: /api/players?name=Foo >>> srting "name" gets value
          => await context.GetFilteredPlayers(name);

        [Route("players")]
        [HttpPost]
        public async Task<Player> AddPlayer([FromBody] Player player) => await context.AddPlayer(player);

        [Route("matches/open")]
        [HttpGet]
        public async Task<IList<Match>> GetMatchesWithNoWinner() => await context.GetIncompleteMatches();

        [Route("matches/generate")]
        [HttpPost]
        public async Task GenerateFirstRound() => await context.GenerateMatchesForNextRound();
    }
}
