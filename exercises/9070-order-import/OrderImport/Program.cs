using OrderImport.Models;
using System;
using System.Threading.Tasks;

namespace OrderImport
{
    public class Program
    {
        static async Task Main(string[] args)
        {
#if DEBUG             
            args[0] = "check";
            args[1] = "customers.txt"; args[2] = "orders.txt";
#endif
            Operations operations = new();

            switch (args[0])
            {
                case "import":
                    await operations.Import(args[1], args[2]);                    
                    break;
                case "clean":
                    await operations.Clean();
                    break;
                case "check":
                    await operations.Check();
                    break;
                case "full":
                    await operations.Full(args[1], args[2]);
                    break;
                default:
                    Console.Error.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
