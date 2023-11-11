using Grpc.Core;
using Grpc.Net.Client;
using MainService.Configs;

namespace MainService.Communicators
{
    public class AnketDbCommunicator
    {
        private readonly Anket.AnketClient _anketClient;

        public AnketDbCommunicator()
        {
            var config = ConfigReader.GetGrpc("AnketDbService").Result;

            GrpcChannel channel = GrpcChannel.ForAddress($"{config.Name}://{config.Host}:{config.Port}", new GrpcChannelOptions() { Credentials = ChannelCredentials.Insecure });
            _anketClient = new Anket.AnketClient(channel);
        }

        public async Task<GetAnketReply> GetAnket()
        {
            return await _anketClient.GetAnketAsync(new GetAnketRequest());
        }
    }
}
