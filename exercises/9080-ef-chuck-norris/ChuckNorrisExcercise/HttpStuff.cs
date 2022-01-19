using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChuckNorrisExcercise
{
    public class HttpStuff
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();
        private static JokeData joke;

        public static async Task<JokeData> GetHttpData()
        {            
            try
            {
                string responseBody = await client.GetStringAsync("https://api.chucknorris.io/jokes/random");
                joke = JsonSerializer.Deserialize<JokeData>(responseBody);
                // var joke = await client.GetFromJsonAsync<JokeData>("https://api.chucknorris.io/jokes/random"); // Throws exceptions
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return joke;
        }
    }
}
