using DataModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Aplicatia de monitorizare in retea a pornit...");
            // Console.ReadLine();
            Comm.Receiver receiver = new Comm.Receiver("127.0.0.1", 3000);
            receiver.DataReceived += ReceivedSomeData;
            receiver.StartListen();
        }

        private static async void ReceivedSomeData(object sender, EventArgs e)
        {
            // Console.WriteLine(sender.ToString());
            switch (Convert.ToInt32(sender.ToString()))
            {
                case 0:
                    Console.WriteLine("Sistem oprit!");
                    break;
                case 1:
                    Console.WriteLine("Galben aprins");
                    break;
                case 2:
                    Console.WriteLine("Galben stins");
                    break;
                case 3:
                    Console.WriteLine("Rosu masini, verde pietoni");
                    break;
                case 4:
                    Console.WriteLine("Galben masini, rosu pietoni");
                    break;
                case 5:
                    Console.WriteLine("Verde masini, rosu pietoni");
                    break;
            }

            var postData = new Stamp(sender.ToString(), DateTime.Now);
            await PutDataAsync(postData);
        }

        public static async Task PutDataAsync(Stamp postData)
        {
            try
            {
                string url = "http://localhost:5040/api/simulator";
                string json = JsonConvert.SerializeObject(postData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                // Optionally, process the response body
                // Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("Request timed out.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e.Message}");
            }
        }
    }
}
