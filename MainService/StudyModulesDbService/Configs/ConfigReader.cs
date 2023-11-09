using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace StudyModulesDbService.Configs;
public static class ConfigReader
{
    public static async Task<GrpcConfig> GetGrpc(string name)
    {
        var json = string.Empty;
        using (var fs = File.OpenRead("Configs/GrpcConfig.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);
        var configJson = JsonConvert.DeserializeObject<List<GrpcConfig>>(json);
        foreach (var VARIABLE in configJson)
        {
            if (name == VARIABLE.Name)
            {
                return VARIABLE;
            }
        }
        return null;
    }
    
} 