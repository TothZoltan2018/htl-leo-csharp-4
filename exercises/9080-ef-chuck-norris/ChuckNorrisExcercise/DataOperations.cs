using ChuckNorrisExcercise.EFDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckNorrisExcercise
{
    public class DataOperations
    {
        public async Task InsertToDB(int numberOfJokes)
        {
            var factory = new ChuckNorrisContextFactory();
            var chuckNorrisContext = factory.CreateDbContext();

            JokeData joke; // = new ();

            // Algorithm 1.
            // Get one joke.
            // Check if categories does not contain "explicit" value AND it is uniqe in DB
            // Get new joke if needed. Try max 10x.
            // Write             
            int storedJokes = 0;

            while (storedJokes != numberOfJokes)
            {
                int attemptCounter = 0;
                while (attemptCounter < 10)
                {
                    // timeout needed
                    do
                    { // while categories is NOT "explicit" Get a random joke
                        joke = await HttpStuff.GetHttpData();
                    } while (joke.Categories.Contains("explicit"));

                    if (chuckNorrisContext.JokeDatas.Where(d => d.ChuckNorrisId == joke.ChuckNorrisId).Count() == 0)
                    { // Not in the DB yet, so insert it
                        chuckNorrisContext.JokeDatas.Add(joke);
                        await chuckNorrisContext.SaveChangesAsync();
                        storedJokes++;
                        break;
                    }
                    else
                    {
                        attemptCounter++;
                    }

                    if (attemptCounter == 10)
                    {
                        Console.WriteLine($"It seems that all the jokes are retrived from the Webpage." +
                            $"Exciting after retrieving {storedJokes}/{numberOfJokes} jokes");
                    }
                }
            } 

           
            







            // Algorithm 2.
            // Build a List<Jokes> in length of 'numberOfJokes' times which to be insert into DB:
            //      Check each recently got joke if they are uniqe among the new ones AND categories does not contain "explicit" value. Also check each of them 
            // Get new joke if needed. Try max 10x.
            // Write 
        }
    }
}
