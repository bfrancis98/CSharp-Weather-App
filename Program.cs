using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;

namespace CSharp_Weather_App;

class Program
{
    static string apiKey = "";
    static readonly HttpClient client = new HttpClient();
    static string lat = "";
    static string lon = "";
    static string units = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter your API key: ");
        apiKey = Console.ReadLine();
        Console.WriteLine("Enter your latitude and longitude (lat,lon): ");
        ParseLatLon(Console.ReadLine());
        Console.WriteLine("Imperial or metric?: ");
        UnitHandler(Console.ReadLine());
    }

    static void ParseLatLon(string value)
    {
        char separator = ',';
        string[] coords = value.Split(separator);
        lat = coords[0];
        lon = coords[1];
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
}
