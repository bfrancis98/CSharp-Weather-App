public class UserInputHandler
{
    public string apiKey = "";
    public string settingsPath = "settings.txt";
    
    public string lat = "";
    public string lon = "";
    public string units = "";
    public bool areSettingsValid = false;

    public void SettingsSetUp()
    {
        if (File.Exists(settingsPath))
        {
            // prob want to check if there is valid data in the settings
            // load variables here
            Console.WriteLine("Settings file found! (" + settingsPath + ")");
            string settings = File.ReadAllText(settingsPath);
            string[] workingSettings = Parser(settings, Environment.NewLine);

            ValidateApiKey(workingSettings[0]);
            string tempCoords = workingSettings[1] + "," + workingSettings[2];
            ValidateCoordinates(tempCoords);
            ValidateUnits(workingSettings[3]);

            SaveSettings();
            areSettingsValid = true;
        }  
        else 
        {
            FileStream fs = File.Create(settingsPath);
            fs.Close();
            
            Console.WriteLine("No settings found! (" + settingsPath + ")");
            UserSetUp();
        }
    }

    string[] Parser(string value, string separator)
    {
        string[] result = value.Split(separator);
        return result;
    }

    void UserSetUp()
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

    bool ValidateApiKey(string value)
    {
        if (value.Length == 32)
        {
            apiKey = value;
            return true;
        } 
        else 
        {
            Console.WriteLine("Invalid API Key. Please enter API Key: ");
            
        }

        return false;
    }

    bool ValidateCoordinates(string value)
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

    bool ValidateCoordinates(float lat, float lon)
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

    bool ValidateUnits(string value)
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
    
    void SaveSettings()
    {
        if (File.Exists(settingsPath))
        {
            string workingString = 
            apiKey + Environment.NewLine +
            lat + Environment.NewLine +
            lon + Environment.NewLine +
            units + Environment.NewLine;

            File.WriteAllText(settingsPath, workingString);
        }
    }
}