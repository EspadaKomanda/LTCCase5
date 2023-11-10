using AuthDbService.Database.Models;
using AuthDbService.Managers;
using Grpc.Core;

namespace AuthDbService.Services
{
    public class AuthDbService : AuthDb.AuthDbBase
    {
        private AuthDbManager _manager = new AuthDbManager();
        public override async Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var reply = new GetUsersReply();
            reply.Users.AddRange( await convertList(await _manager.GetUsers()));
            return reply;
        }
        private async Task<List<GetReply>> convertList(List<UserModel> users)
        {
            List<UserModel> result = new List<UserModel>();
            foreach (var VARIABLE in users)
            {
                var user = new GetReply()
                {
                   About = VARIABLE.about,
                   Avatar = VARIABLE.avatar,
                   DateOfBirth = VARIABLE.dateOfBirth,
                   Email = VARIABLE.email,
                   FirstName = VARIABLE.firstName,
                   LastName = VARIABLE.lastName,
                   Password = VARIABLE.password,
                   Patronymic = VARIABLE.patronymic,
                   Phone = VARIABLE.phone,
                   Position = VARIABLE.position,
                   Uuid = VARIABLE.uuid.ToString(),
                   Role = VARIABLE.role,
                   Telegram = VARIABLE.telegram
                };
                anket.QuestionVariants.AddRange(VARIABLE.answerVariants.Split("-_-"));
                result.Add(anket);
            }
            return result;
        }
        public async override Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var result = await _manager.AddUser(new UserModel()
            {
                email = request.Email,
                phone = request.Phone,
                telegram = request.Telegram,
                firstName = request.FirstName,
                lastName = request.LastName,
                password = request.Password,
                patronymic = request.Patronymic,
                position = request.Position,
                role = request.Role,
                about = request.About,
                avatar = request.Avatar,
                dateOfBirth = request.DateOfBirth
            });
            return await Task.FromResult(new CreateUserReply()
            {
                Result = result
            });
        }

        public async override Task<DeleteUserByEmailReply> DeleteUserByEmail(DeleteUserByEmailRequest request, ServerCallContext context)
        {
            var result = await _manager.DeleteUserByEmail(new UserModel()
            {
                email = request.Email
            });
            return await Task.FromResult(new DeleteUserByEmailReply()
            {
                Result = result
            });
        }

        public async override Task<DeleteUserByIdReply> DeleteUserById(DeleteUserByIdRequest request, ServerCallContext context)
        {
            Guid accountId;
            Guid.TryParse(request.Uuid, out accountId);
            var result = await _manager.DeleteUserById(new UserModel()
            {
                uuid = accountId
            });
            return await Task.FromResult(new DeleteUserByIdReply()
            {
               Reply = result
            });
        }

        public async override Task<GetReply> GetUserByEmail(GetUserByEmailRequest request, ServerCallContext context)
        {
            var result = await _manager.GetUserByEmail(new UserModel()
            {
                email = request.Email
            });
            return await Task.FromResult(new GetReply()
            {
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
                Uuid = result.uuid.ToString(),
                DateOfBirth = result.dateOfBirth
            });
        }

        public async override Task<GetReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            Guid accountId;
            Guid.TryParse(request.Uuid, out accountId);
            var result = await _manager.GetUserById(new UserModel()
            {
                uuid = accountId
            });
            return await Task.FromResult(new GetReply()
            {
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
                Uuid = result.uuid.ToString(),
                DateOfBirth = result.dateOfBirth
            });
        }

        public async override Task<ModifyReply> ModifyUserByEmail(ModifyUserByEmailRequest request, ServerCallContext context)
        {
            var result = await _manager.ModifyUserByEmail(new UserModel()
            {
                email = request.Email,
                phone = request.Phone,
                telegram = request.Telegram,
                firstName = request.FirstName,
                lastName = request.LastName,
                password = request.Password,
                patronymic = request.Patronymic,
                position = request.Position,
                role = request.Role,
                about = request.About,
                avatar = request.Avatar,
                dateOfBirth = request.DateOfBirth
            });
            return await Task.FromResult(new ModifyReply()
            {
                Reply = result
            });
        }

        public async override Task<ModifyReply> ModifyUserById(ModifyUserByIdRequest request, ServerCallContext context)
        {
            Guid accountId;
            Guid.TryParse(request.Uuid, out accountId);
            var result = await _manager.ModifyUserById(new UserModel()
            {
                email = request.Email,
                phone = request.Phone,
                telegram = request.Telegram,
                firstName = request.FirstName,
                lastName = request.LastName,
                password = request.Password,
                patronymic = request.Patronymic,
                position = request.Position,
                role = request.Role,
                about = request.About,
                avatar = request.Avatar,
                uuid = accountId,
                dateOfBirth = request.DateOfBirth
            });
            return await Task.FromResult(new ModifyReply()
            {
                Reply = result
            });
        }
    }
}
