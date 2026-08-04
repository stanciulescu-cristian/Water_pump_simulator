using DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleConsumer
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(300) // Increase timeout as needed
        };

        static async Task Main(string[] args)
        {
            Console.WriteLine("Aplicatia consumator a pornit si asteapta date ...");

            while (true)
            {
                var data = await GetDataAsync();
                Console.Clear();
                Console.WriteLine($"Date primite: {data.Count}");
                await Task.Delay(2500);
            }
        }

        public static async Task<List<Stamp>> GetDataAsync()
        {
            string url = @"http://localhost:5040/api/simulator";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var userData = JsonConvert.DeserializeObject<List<Stamp>>(responseBody);
                return userData;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Request timed out.");
                // Handle timeout or retry logic here
                return new List<Stamp>(); // Return an empty list or handle as needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Stamp>(); // Return an empty list or handle as needed
            }
        }
    }
}
