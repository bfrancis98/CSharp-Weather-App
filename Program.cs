using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.IO;
using System.Text;

namespace CSharp_Weather_App;

class Program
{
    static string apiKey = "";
    static string settingsPath = "";
    static readonly HttpClient client = new HttpClient();
    static string lat = "";
    static string lon = "";
    static string units = "";

    static async Task Main(string[] args)
    {
        SettingsSetUp();
        /*
        
        */
        
    }

    // not a fan of using string separator, but new line character is a string
    // wonder if i should validate separator...
    static string[] Parser(string value, string separator)
    {
        string[] result = value.Split(separator);
        return result;
    }

    static void UnitHandler(string value)
    {
        string workingString = value.ToLower();
    
        switch(workingString)
        {
            case "imperial":
                units = "imperial";
                break;
            case "metric":
                units = "metric";
                break;
        }
    }

    static void SettingsSetUp()
    {
        if (File.Exists("settings.txt"))
        {
            // prob want to check if there is valid data in the settings
            // load variables here
            string settings = File.ReadAllText("settings.txt");
            string[] workingSettings = Parser(settings, Environment.NewLine);
            apiKey = workingSettings[0];
            lat = workingSettings[1];
            lon = workingSettings[2];
            units = workingSettings[3];

        }  
        else 
        {
            File.Create("settings.txt");
            UserSetUp();
        }
    }


    static void UserSetUp()
    {
        // need error handling

        Console.WriteLine("Enter your API key: ");
        apiKey = Console.ReadLine();
        Console.WriteLine("Enter your latitude and longitude (lat,lon): ");
        Parser(Console.ReadLine(), ",");
        Console.WriteLine("Imperial or metric?: ");
        UnitHandler(Console.ReadLine());
    }
}
/*
Settings looks like this:
L1: apikey
L2: lat
L3: lon
L4: units

should separate with new line characters
to make it more complicated, i could label the different lines
and remove the labels from the lines to get the values...
*/