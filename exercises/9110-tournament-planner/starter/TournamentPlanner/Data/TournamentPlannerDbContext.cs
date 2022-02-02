using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

// README: https://github.com/TothZoltan2018/htl-leo-csharp-4/tree/master/exercises/9110-tournament-planner
// First, open a packet manager console and create DB: dotnet ef database update FKsAddedInMatches
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
                        
            Match match = new() { Player1 = player1, Player2 = player2, Round = round, Winner = null };
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
            Player playerWinner = player == PlayerNumber.Player1 ? match.Player1 : match.Player2; 
            match.Winner = await this.Players.FindAsync(playerWinner.ID);
            //this.Matches.Update(match); // Not needed 
            
            await this.SaveChangesAsync();
            return match;
        }

        /// <summary>
        /// Get a list of all matches that do not have a winner yet
        /// </summary>
        /// <returns>List of all found matches</returns>    
        public async Task<IList<Match>> GetIncompleteMatches()
        {
            // todo player data missing
            return await this.Matches                
                .Where(m => m.Winner == null)
                //.Include(m => m.Player1)
                .ToListAsync();
        }

        /// <summary>
        /// Delete everything (matches, players)
        /// </summary>
        public async Task DeleteEverything()
        {
            using var transaction = await this.Database.BeginTransactionAsync();
            try
            {
                this.Matches.RemoveRange(Matches);
                this.Players.RemoveRange(Players);

                await this.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Error.WriteLine($"Something bad happened: {ex.Message}");
                await transaction.RollbackAsync();
            }            
        }

        /// <summary>
        /// Get a list of all players whose name contains <paramref name="playerFilter"/>
        /// </summary>
        /// <param name="playerFilter">Player filter. If null, all players must be returned</param>
        /// <returns>List of all found players</returns>
        public async Task<IList<Player>> GetFilteredPlayers(string playerFilter = null)
        {            
            if (playerFilter != null)
            {
                return await this.Players.Where(p => p.Name.Contains(playerFilter)).ToListAsync();
            }
            else
            {
                return await this.Players.ToListAsync();
            }
        }

        /// <summary>
        /// Generate match records for the next round
        /// </summary>
        /// <exception cref="InvalidOperationException">Error while generating match records</exception>
        public async Task GenerateMatchesForNextRound()
        {
            using var transaction = await this.Database.BeginTransactionAsync();

            if (await this.Players.CountAsync() != 32)
            {
                throw new InvalidOperationException("The number of players is not 32.");
            }

            if (await this.Matches.AnyAsync(m => m.Winner == null))
            {
                throw new InvalidOperationException("Not all the winners are set.");
            }

            int nextRound;
            Random r = new();            

            try
            {
                var numberOfMatches = await this.Matches.CountAsync();
                switch (numberOfMatches)
                {
                    case 0:
                        nextRound = 1;
                        break;
                    case 16:
                        nextRound = 2;
                        break;
                    case 24:
                        nextRound = 3;
                        break;
                    case 28:
                        nextRound = 4;
                        break;
                    case 30:
                        nextRound = 5;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid number of rounds.");
                }

                if (nextRound == 1) // generate 16 matches between random players. Winner needs to be set?
                {
                    List<Player> players = await this.Players.ToListAsync();
                    int rndMax = 32;
                    await SetMatch(nextRound, r, players, rndMax);

                }
                else // generate matches between random winners of the previous round.
                {
                    var x = await this.Matches.Where(m => m.Round == nextRound - 1).ToListAsync();

                    List<Player> players = await this.Matches
                        .Where(m => m.Round == nextRound - 1)
                        .Select(m => m.Winner).ToListAsync();

                    int rndMax = players.Count;
                    await SetMatch(nextRound, r, players, rndMax);
                }

                static int SetRandomPlayer(Random r, List<Player> players, ref int rndMax)
                {
                    var playerIndex = r.Next(0, rndMax);
                    int player = players[playerIndex].ID;
                    players.RemoveAt(playerIndex);
                    rndMax--;
                    return player;
                }

                async Task SetMatch(int nextRound, Random r, List<Player> players, int rndMax)
                {
                    while (rndMax > 1)
                    {
                        int player1 = SetRandomPlayer(r, players, ref rndMax);
                        int player2 = SetRandomPlayer(r, players, ref rndMax);

                        var match = (await this.AddMatch(player1, player2, nextRound)).ID;
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Error.WriteLine($"Something bad happened: {ex.Message}");
                await transaction.RollbackAsync();
            }
        }
    }
}
