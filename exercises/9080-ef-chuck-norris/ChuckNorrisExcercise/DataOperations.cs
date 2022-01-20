using ChuckNorrisExcercise.EFDataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckNorrisExcercise
{
    public class DataOperations
    {
        private const int maxAttemptToGetUniqueJoke = 50;
        ChuckNorrisContextFactory factory = new ChuckNorrisContextFactory();
        
        public async Task InsertToDB(int numberOfJokes)
        {
            using ChuckNorrisContext chuckNorrisContext = factory.CreateDbContext();

            JokeData joke = new ();
    
            int storedJokes = 0;

            while (storedJokes != numberOfJokes)
            {
                int attemptCounter = 0;
                while (attemptCounter < maxAttemptToGetUniqueJoke)
                {
                    bool isExplicit = true;
                    while (isExplicit)
                    {// 'Jokes in explicit' category will not be written into DB
                        int nonExplicitAttempts = 1;
                        joke = await HttpStuff.GetHttpData();
                        isExplicit = joke.Categories.Contains("explicit");
                        if (isExplicit) {
                            Console.WriteLine($"For the {nonExplicitAttempts}. attempt the retrieved joke was in the category 'explicit'. Retreiving an other one." );
                            nonExplicitAttempts++;
                        }
                    }

                    if ( ! chuckNorrisContext.JokeDatas.Where(d => d.ChuckNorrisId == joke.ChuckNorrisId).Any())                    
                    { // Not in the DB yet, so insert it
                        chuckNorrisContext.JokeDatas.Add(joke);
                        await chuckNorrisContext.SaveChangesAsync();
                        storedJokes++;
                        Console.WriteLine($"{storedJokes}. joke is iserted to DB.");
                        break;
                    }
                    else
                    {
                        attemptCounter++;
                        Console.WriteLine($"!!!!!!!!!!! {attemptCounter}. attempt to get a unique joke !!!!!!!!!!!");
                        if (attemptCounter == maxAttemptToGetUniqueJoke)
                        {
                            Console.WriteLine($"It seems that all the jokes are retrived from the Webpage." +
                                $"Exiting after retrieving {storedJokes}/{numberOfJokes} jokes");
                            return;
                        }
                    } 
                }
            } 
        }

        public async Task Clear()
        {
            using ChuckNorrisContext chuckNorrisContext = factory.CreateDbContext();            

            await chuckNorrisContext.Database.ExecuteSqlRawAsync("DELETE FROM JokeDatas");
            Console.WriteLine("All Data is cleared from DB. And that is not a joke!");
        }
    }
}
