using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:50011/");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            Console.ReadKey();
        }
    }
}
