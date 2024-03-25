using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharp_Weather_App;

class Program
{
    static async Task Main(string[] args)
    {
        UserInputHandler userInputHandler = new UserInputHandler();
        WeatherData weatherData = new WeatherData();
        userInputHandler.SettingsSetUp();
        if(userInputHandler.areSettingsValid)
        {
            await weatherData.GetWeatherData(
                userInputHandler.lat,
                userInputHandler.lon,
                userInputHandler.units,
                userInputHandler.apiKey);
        }

    }
}