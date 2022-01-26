using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentPlanner.Data
{
    public enum PlayerNumber { Player1 = 1, Player2 = 2 };

    public class TournamentPlannerDbContext : DbContext
    {        
        public TournamentPlannerDbContext(DbContextOptions<TournamentPlannerDbContext> options)
            : base(options)
        { }

        public DbSet<Player> Players { get; set; }

        public DbSet<Match> Matches { get; set; }

        /// <summary>
        /// Adds a new player to the player table
        /// </summary>
        /// <param name="newPlayer">Player to add</param>
        /// <returns>Player after it has been added to the DB</returns>
        public async Task<Player> AddPlayer(Player newPlayer)
        {
            this.Add(newPlayer);
            await this.SaveChangesAsync();
            return newPlayer;
        }

        /// <summary>
        /// Adds a match between two players
        /// </summary>
        /// <param name="player1Id">ID of player 1</param>
        /// <param name="player2Id">ID of player 2</param>
        /// <param name="round">Number of the round</param>
        /// <returns>Generated match after it has been added to the DB</returns>
        public async Task<Match> AddMatch(int player1Id, int player2Id, int round)
        {
            Player player1 = await this.Players.FindAsync(player1Id);
            Player player2 = await this.Players.FindAsync(player2Id);

            Match match = new() { Players = new List<Player>() { player1, player2 }, Round = round };
            this.Add(match);
            await this.SaveChangesAsync();
            return match;
        }

        /// <summary>
        /// Set winner of an existing game
        /// </summary>
        /// <param name="matchId">ID of the match to update</param>
        /// <param name="player">Player who has won the match</param>
        /// <returns>Match after it has been updated in the DB</returns>
        public async Task<Match> SetWinner(int matchId, PlayerNumber player)
        {
            Match match = await this.Matches.FindAsync(matchId);
            match.Winner = await this.Players.FindAsync((int)player);

            this.Matches.Update(match);
            await this.SaveChangesAsync();
            return match;
        }

        /// <summary>
        /// Get a list of all matches that do not have a winner yet
        /// </summary>
        /// <returns>List of all found matches</returns>
        public Task<IList<Match>> GetIncompleteMatches()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete everything (matches, players)
        /// </summary>
        public async Task DeleteEverything()
        {
            this.Matches.RemoveRange(Matches);
            this.Players.RemoveRange(Players);

            await this.SaveChangesAsync();
        }

        /// <summary>
        /// Get a list of all players whose name contains <paramref name="playerFilter"/>
        /// </summary>
        /// <param name="playerFilter">Player filter. If null, all players must be returned</param>
        /// <returns>List of all found players</returns>
        public Task<IList<Player>> GetFilteredPlayers(string playerFilter = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate match records for the next round
        /// </summary>
        /// <exception cref="InvalidOperationException">Error while generating match records</exception>
        public Task GenerateMatchesForNextRound()
        {
            throw new NotImplementedException();
        }
    }
}
