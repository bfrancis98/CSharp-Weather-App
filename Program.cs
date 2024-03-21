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
    static string apiKey = "";
    static string settingsPath = "";
    static readonly HttpClient client = new HttpClient();
    static string lat = "";
    static string lon = "";
    static string units = "";
    static bool areSettingsValid = false;

    static async Task Main(string[] args)
    {
        SettingsSetUp();
        if(areSettingsValid)
        {
            GetWeatherData();
        }

    }

    // not a fan of using string separator, but new line character is a string
    // wonder if i should validate separator...
    static string[] Parser(string value, string separator)
    {
        string[] result = value.Split(separator);
        return result;
    }

    static void SettingsSetUp()
    {
        if (File.Exists("settings.txt"))
        {
            // prob want to check if there is valid data in the settings
            // load variables here
            Console.WriteLine("Settings file found!");
            string settings = File.ReadAllText("settings.txt");
            string[] workingSettings = Parser(settings, Environment.NewLine);

            ValidateApiKey(workingSettings[0]);
            string tempCoords = workingSettings[1] + "," + workingSettings[2];
            ValidateCoordinates(tempCoords);
            ValidateUnits(workingSettings[3]);

            SaveSettings();
        }  
        else 
        {
            FileStream fs = File.Create("settings.txt");
            fs.Close();
            
            Console.WriteLine("No settings found!");
            UserSetUp();
        }
    }


    static void UserSetUp()
    {   
        Console.WriteLine("Enter your API key: ");
        string tempApiKey = Console.ReadLine();
        ValidateApiKey(tempApiKey);
        
        Console.WriteLine("Enter your latitude and longitude (lat,lon): ");
        string tempCoords = Console.ReadLine();
        ValidateCoordinates(tempCoords);
        
        
        Console.WriteLine("Imperial or metric?: ");
        string tempUnits = Console.ReadLine();
        ValidateUnits(tempUnits);

        areSettingsValid = true;
        SaveSettings();
    }

    static async void GetWeatherData()
    {
        try
        {
            string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat="+ lat + "&lon="+ lon + "&appid=" + apiKey;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            // check for if api key is invalid here
        }
    }

    static bool ValidateApiKey(string value)
    {
        if (value.Length == 32 && value.All(char.IsLower))
        {
            apiKey = value;
            return true;
        } 
        else 
        {
            Console.WriteLine("Invalid API Key. Please enter API Key: ");
            ValidateApiKey(Console.ReadLine());
        }

        return false;
    }

    static bool ValidateCoordinates(string value)
    {
        string[] workingCoords = Parser(value, ",");
        bool parseLat = float.TryParse(workingCoords[0], out float tempLat);
        bool parseLon = float.TryParse(workingCoords[1], out float tempLon);
        
        if (parseLat && parseLon)
        {
            if (tempLat >= -90 && tempLat <= 90)
            {
                if (tempLon >= -180 && tempLon <= 180)
                {
                    lat = tempLat.ToString();
                    lon = tempLon.ToString();
                    return true;
                }
            }
        }

        Console.WriteLine("Coordinates invalid. Please re-enter coordinates (lat,lon): ");
        ValidateCoordinates(Console.ReadLine());
        return false;
    }

    static bool ValidateCoordinates(float lat, float lon)
    {
        if (lat >= -90 && lat <= 90)
        {
            if(lon >= -180 && lon >= 180)
            {
                return true;
            }
        }
        
        return false;
    }

    static bool ValidateUnits(string value)
    {
        string workingString = value.ToLower();

        switch(workingString)
        {
            case "imperial":
                units = "imperial";
                return true;
            case "metric":
                units = "metric";
                return true;
            default:
                // ask user for the units again
                Console.WriteLine("Unit is invalid. Please enter imperial or metric: ");
                ValidateUnits(Console.ReadLine());
                return false;
        }

    }

    static void SaveSettings()
    {
        if (File.Exists("settings.txt"))
        {
            string workingString = 
            apiKey + Environment.NewLine +
            lat + Environment.NewLine +
            lon + Environment.NewLine +
            units + Environment.NewLine;

            File.WriteAllText("settings.txt", workingString);
        }
    }
}
/*
api key must be 32 characters
api key must be all lower case

lat must be between -90 and 90
lon must be between -180 and 180

---
check for settings
if settings found
validate api
if true: save
if false: prompt user to enter new api key
validate api


*/