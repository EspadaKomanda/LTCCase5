using Grpc.Core;
using Grpc.Net.Client;
using CompanyAboutDbService.Configs;

namespace CompanyAboutDbService.Communicators
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

        public async Task<UrlReply> GetUrl(string RequestName)
        {
            return await _configClient.GetUrlAsync(new UrlRequest() { UrlName = RequestName });
        }

        public async Task<ClientReply> GetClient()
        {
            return await _configClient.GetClientInfoAsync(new ClientRequest());
        }

        public async Task<DbReply> GetDb(string dbname)
        {
            return await _configClient.GetDbAsync(new DbRequest(){Name = dbname});
        }
    }
}
