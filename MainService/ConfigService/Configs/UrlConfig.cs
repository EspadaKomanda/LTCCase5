using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConfigService.Configs;

public class UrlConfig
{
    [JsonProperty("RequestName")]
    public string RequestName { get;  set; }
    [JsonProperty("Url")]
    public string Url{ get;  set; }
}