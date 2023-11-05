using System.Text;
using System.Text.RegularExpressions;
using ConfigService.Services;
using Newtonsoft.Json;

namespace ConfigService.Configs;

public static class ConfigReader
{
    public static async Task<DbConfig> GetDb(string name)
    {
        var json = string.Empty;
        using (var fs = File.OpenRead("Configs/DbConfig.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);
        var configJson = JsonConvert.DeserializeObject<List<DbConfig>>(json);
        foreach (var VARIABLE in configJson)
        {
            if (name == VARIABLE.ServiceName)
            {
                return VARIABLE;
            }
        }
        return null;
    }

    public static async Task<UrlConfig> GetUrl(string name)
    {
        var json = string.Empty;
        using (var fs = File.OpenRead("Configs/UrlConfig.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);
        var configJson = JsonConvert.DeserializeObject<List<UrlConfig>>(json);
        foreach (var VARIABLE in configJson)
        {
            if (name == VARIABLE.RequestName)
            {
                return VARIABLE;
            }
        }
        return null;
    }

  
    public static async Task<AuthConfig> GetAuth()
    {
        var json = string.Empty;
        using (var fs = File.OpenRead("AuthConfig.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);
        var configJson = JsonConvert.DeserializeObject<AuthConfig>(json);
        return configJson;
    }

}