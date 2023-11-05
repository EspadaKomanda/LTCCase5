using Newtonsoft.Json;

namespace ConfigService.Configs;

public class DbConfig
{
    [JsonProperty("ServiceName")]
    public string ServiceName { get;  set; }
    [JsonProperty("Server")]
    public string Server{ get;  set; }
    [JsonProperty("User")]
    public string User { get;  set; }
    [JsonProperty("Password")]
    public string Password { get; set; }
    [JsonProperty("DatabaseName")] 
    public string DatabaseName { get;  set; }
}