using Bitfinex.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BitfinexClient client = new BitfinexClient();
            client.SetApiCredentials("", "");
            var balance = client.GetWallets();


            var xxx = client.GetSymbols();

            Console.Read();
        }
    }
}
