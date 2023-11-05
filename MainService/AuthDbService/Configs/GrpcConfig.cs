using Newtonsoft.Json;

namespace AuthDbService.Configs;

public class GrpcConfig
{
    [JsonProperty("Name")]
    public string Name{ get; private set; }
    [JsonProperty("Host")]
    public string Host { get; private set; }
    [JsonProperty("Port")]
    public int Port { get; private set; }
}