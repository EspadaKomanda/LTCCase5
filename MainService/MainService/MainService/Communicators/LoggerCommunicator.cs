using Grpc.Core;
using Grpc.Net.Client;
using MainService.Configs;

namespace MainService.Communicators
{
    public class LoggerCommunicator
    {
        private readonly Log.LogClient _logClient;

        public LoggerCommunicator()
        {
            var config = ConfigReader.GetGrpc("LogService").Result;

            GrpcChannel channel = GrpcChannel.ForAddress($"{config.Name}://{config.Host}:{config.Port}", new GrpcChannelOptions() { Credentials = ChannelCredentials.Insecure });
            _logClient = new Log.LogClient(channel);
        }

        public async Task<GetLogsReply> GetLogs()
        {
            return _logClient.GetLogs(new GetLogsRequest());
        }

        public async Task<CreateLogReply> CreateLog(string logLevel, string message)
        {
            return _logClient.CreateLog(new CreateLogRequest()
            {
                LogLevel = logLevel,
                Message = message
            });
        }
    }
}
