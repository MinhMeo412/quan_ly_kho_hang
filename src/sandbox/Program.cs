using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string weather = await GetWeather();
        Console.WriteLine(weather);
    }

    public static async Task<string> GetWeather()
    {
        string url = "https://wttr.in/?0?A?d"; // This returns weather condition and temperature

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string response = await client.GetStringAsync(url);
                return response;
            }
            catch (Exception ex)
            {
               return $"Error: {ex.Message}";
            }
        }
    }
}
