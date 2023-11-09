using Grpc.Core;
using Grpc.Net.Client;
using MainService.Configs;

namespace MainService.Communicators
{
    public class AuthDbCommunicator 
    {
        private readonly AuthDb.AuthDbClient _authDbClient;

        public AuthDbCommunicator()
        {
            var config = ConfigReader.GetGrpc("AuthDbService").Result;

            GrpcChannel channel = GrpcChannel.ForAddress($"{config.Name}://{config.Host}:{config.Port}", new GrpcChannelOptions() { Credentials = ChannelCredentials.Insecure });
            _authDbClient = new AuthDb.AuthDbClient(channel);
        }

        public async Task<GetReply> GetUserByEmail(string email)
        {
            return await _authDbClient.GetUserByEmailAsync(new GetUserByEmailRequest()
            {
                Email = email
            });
        }

        public async Task<ModifyReply> Modify(Guid userId,string avatarUrl)
        {
            return await _authDbClient.ModifyUserByIdAsync(new ModifyUserByIdRequest()
            {
                Uuid = userId.ToString(),
                About = "",
                Avatar = avatarUrl,
                DateOfBirth = "",
                Email = "",
                FirstName = "",
                LastName = "",
                Password = "",
                Patronymic = "",
                Phone = "",
                Role = "",

            });
        }
    }
}
