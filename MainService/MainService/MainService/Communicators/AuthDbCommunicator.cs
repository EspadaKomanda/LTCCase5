using Grpc.Core;
using Grpc.Net.Client;
using MainService.Configs;
using MainService.Pages.Models;

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

        public async Task<ModifyReply> Modify(UserModel result)
        {
            return await _authDbClient.ModifyUserByIdAsync(new ModifyUserByIdRequest()
            {
                Uuid = result.uuid.ToString(),
                Email = result.email,
                Phone = result.phone,
                Telegram = result.telegram,
                FirstName = result.firstName,
                LastName = result.lastName,
                Password = result.password,
                Patronymic = result.patronymic,
                Position = result.position,
                Role = result.role,
                About = result.about,
                Avatar = result.avatar,
                DateOfBirth = result.dateOfBirth
            });
        }
    }
}
