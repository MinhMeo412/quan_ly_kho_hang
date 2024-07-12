using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // string url = "https://wttr.in/?0?A"; // This returns weather condition and temperature

        // using (HttpClient client = new HttpClient())
        // {
        //     try
        //     {
        //         string response = await client.GetStringAsync(url);
        //         Console.WriteLine($"Current weather: {response}");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error: {ex.Message}");
        //     }
        // }
        // Console.WriteLine(GetWeather());
        await GetWeather();
    }

    public static async Task GetWeather()
    {
       string url = "https://wttr.in/?0?A"; // This returns weather condition and temperature

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string response = await client.GetStringAsync(url);
                Console.WriteLine($"Current weather: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
