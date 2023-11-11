using Grpc.Core;
using Grpc.Net.Client;
using MainService.Configs;
using MainService.Pages;
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

        public async Task<GetUsersReply> getUsers()
        {
            return await _authDbClient.GetUsersAsync(new GetUsersRequest());
        }

        public async Task<CreateUserReply> CreateUser(CreationUserModel model)
        {
            string[] name = model.name.Split(" ");
            return await _authDbClient.CreateUserAsync(new CreateUserRequest()
            {
                Password = model.password,
                Patronymic = name[name.Length - 1],
                Role = "Employee",
                Email = model.email,
                About = " ",
                Avatar = " ",
                DateOfBirth = " ",
                FirstName = name[1],
                LastName = name[0],
                Phone = model.phone,
                Position = model.position,
                Telegram = " ",
                
            });
        }
        public async Task<GetReply> GetUserByEmail(string email)
        {
            return await _authDbClient.GetUserByEmailAsync(new GetUserByEmailRequest()
            {
                Email = email
            });
        }

        public async Task<GetReply> GetUserByUuId(string uuId)
        {
            return await _authDbClient.GetUserByIdAsync(new GetUserByIdRequest()
            {
                Uuid = uuId
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
