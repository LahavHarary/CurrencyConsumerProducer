using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace shared_resources.Services;

public static class ConfigHelperService
{
    private static string _baseDirectory;
    private static string _configDirectory;
    private static string _configFile;
    
    static ConfigHelperService()
    {
        _baseDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("CurrencyConsumerProducer"));
        _configDirectory = Path.Combine(_baseDirectory, "CurrencyConsumerProducer/shared_resources/"); 
        _configFile = Path.Combine(_configDirectory, "config.json");    
    }
    
    public static string? GetData(string dataToGet)
    {
        string? res = null;
        if (File.Exists(_configFile))
        {
            string configContent = File.ReadAllText(_configFile);
            var configJson = JObject.Parse(configContent);

            res = configJson["MySettings"]?[dataToGet]?.ToString();
        }

        return res;
    }

    public static string? GetConfigDirectory()
    {
        return _configDirectory;
    }
}