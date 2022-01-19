using System;
using System.Threading.Tasks;
using System.Linq;

namespace ChuckNorrisExcercise
{
    //Excercise from:
    //https://github.com/TothZoltan2018/htl-leo-csharp-4/tree/master/exercises/9080-ef-chuck-norris
    public class Program
    {        
        static async Task Main(string[] args)
        {            
#if DEBUG            
            args = new string[] { "3"};
#endif
            
            //int numOfJokes;

            if (args.Length != 0 && args[0] == "clear")
            {
                // delete data
            }
            else
            {
                int numOfJokes = ParseForNumberOfJokes(args);
                DataOperations dataOperations = new();
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
                if ((int.TryParse(args[0], out numOfJokes) && numOfJokes > 0 && numOfJokes <= 10) == false)
                {
                    Console.WriteLine("Bad number of jokes argument. It should be an integer value between 1 and 10! ");
                }
            }

            return numOfJokes;
        }
    }
}
