using System;
using System.Threading.Tasks;
using System.Linq;

namespace ChuckNorrisExcercise
{
    //Excercise from:
    //https://github.com/TothZoltan2018/htl-leo-csharp-4/tree/master/exercises/9080-ef-chuck-norris
    public class Program
    {
        const int maxRetrieveableJokes = 10000;
        static DataOperations dataOperations = new();
        static async Task Main(string[] args)
        {
#if DEBUG
            if (args == null)
            {
                args = new string[] { "500" };
                //args = new string[] { "clear" };
            }
#endif
      
            if (args.Length != 0 && args[0] == "clear")
            {
                await dataOperations.Clear();
            }
            else
            {
                int numOfJokes = ParseForNumberOfJokes(args);
                
                await dataOperations.InsertToDB(numOfJokes);
            }            
        }

        private static int ParseForNumberOfJokes(string[] args)
        {
            int numOfJokes;
            
            if (args.Length == 0)
            {
                numOfJokes = 5;
            }
            else
            {
                if ((int.TryParse(args[0], out numOfJokes) && numOfJokes > 0 && numOfJokes <= maxRetrieveableJokes) == false)
                {
                    Console.WriteLine($"Bad number of jokes argument. It should be an integer value between 1 and {maxRetrieveableJokes}! ");
                    return 0;
                }
            }

            return numOfJokes;
        }
    }
}
