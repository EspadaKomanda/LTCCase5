using Grpc.Core;
using LogService.Utils;
using LogLevel = LogService.Utils.LogLevel;

namespace LogService.Services
{
    public class LoggerService : Log.LogBase
    {
        private Logger _logger = new Logger("/logs");
        public async override Task<CreateLogReply> CreateLog(CreateLogRequest request, ServerCallContext context)
        {
            if (request.LogLevel == "Authorization")
            {
                 await _logger.LogAsync(LogLevel.Authorization,request.Message);
            }
            else if (request.LogLevel == "Creation")
            {
                await _logger.LogAsync(LogLevel.Creation,request.Message);
            }
            else
            {
                await _logger.LogAsync(LogLevel.Deletion, request.Message);
            }
            return await Task.FromResult(new CreateLogReply());
        }

        public override async Task<GetLogsReply> GetLogs(GetLogsRequest request, ServerCallContext context)
        {
            var result = await _logger.ReadLogsAsync(DateTime.MinValue, DateTime.Now);
            var reply = new GetLogsReply();
            reply.Logs.AddRange(await convertList(result));
            return reply;
        }
        private async Task<List<Logs>> convertList(List<LogEntry> logs)
        {
            List<Logs> result = new List<Logs>();
            foreach (var VARIABLE in logs)
            {
                var log = new Logs()
                {
                  Message = VARIABLE.Message,
                  LogLevel = VARIABLE.Level.ToString(),
                  Timestamp = VARIABLE.Timestamp.ToString()
                };
                result.Add(log);
            }
            return result;
        }
    }
}
