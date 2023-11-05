using MainService.Configs;
using Grpc.Core;
using Grpc.Net.Client;

namespace MainService.Communicators
{
    public class ConfigCommunicator : Config.ConfigClient
    {
        private readonly Config.ConfigClient _configClient;
        public ConfigCommunicator()
        {
            var config = ConfigReader.GetGrpc("ConfigService").Result;

            GrpcChannel channel = GrpcChannel.ForAddress($"{config.Name}://{config.Host}:{config.Port}", new GrpcChannelOptions() { Credentials = ChannelCredentials.Insecure });
            _configClient = new Config.ConfigClient(channel);
        }

        public async Task<AReply> GetAuth()
        {
            return await _configClient.GetAuthAsync(new ARequest());

        }
    }
}
