using Newtonsoft.Json;

namespace ConfigService.Configs;

public class ClientConfig
{
    [JsonProperty("ClientId")]
    public string ClietnId { get; set; }
    [JsonProperty("ClientSecret")]
    public string ClientSecret { get; set; }
}