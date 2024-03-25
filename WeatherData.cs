public class WeatherData
{
    readonly HttpClient client = new HttpClient();

    public async Task GetWeatherData(string lat, string lon, string units, string apiKey)
    {
        try
        {
            string apiUrl = "https://api.openweathermap.org/data/2.5/weather?lat="+ lat + "&lon="+ lon + "&appid=" + apiKey + "&units=" + units;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}