﻿using AuthDbService.Database.Models;
using AuthDbService.Managers;
using Grpc.Core;

namespace AuthDbService.Services
{
    public class AuthDbService : AuthDb.AuthDbBase
    {
        private AuthDbManager _manager = new AuthDbManager();
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
                Uuid = result.uuid.ToString()
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
                Uuid = result.uuid.ToString()
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
                uuid = accountId
            });
            return await Task.FromResult(new ModifyReply()
            {
                Reply = result
            });
        }
    }
}
