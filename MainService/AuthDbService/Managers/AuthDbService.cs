using AuthDbService.Database;
using AuthDbService.Database.Models;
using Grpc.Core;

namespace AuthDbService.Managers
{
    public class AuthDbService
    {
        public async Task<string> AddUser(UserModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var state = await ctx.users.AddAsync(new UserModel()
                    {
                        uuid = request.uuid,
                        email = request.email,
                        firstName = request.firstName,
                        lastName = request.lastName,
                        patronymic = request.patronymic,
                        password = request.password,
                        role = request.role
                    });
                    ctx.SaveChanges();
                    if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        return "";
                    }
                    //Cringe but we wanted a string here
                    return "EntityState.Added is false";
                }
            }
            catch (Exception ex) 
            {
                return ex.ToString();
            }
        }

        public async Task<string> DeleteUser(UserModel request)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var user = await ctx.users.FindAsync(request.uuid);
                if (user != null)
                {
                    ctx.users.Remove(user);
                    await ctx.SaveChangesAsync();
                    return "";
                }

                return String.Format("User ({0} does not exist", request.uuid);
            }
        }

        public async Task<string> ModifyUser(UserModel request)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var user = await ctx.users.FindAsync(request.uuid);
                if (user != null)
                {
                    user.email = request.email ?? user.email;
                    user.firstName = request.firstName ?? user.firstName;
                    user.lastName = request.lastName ?? user.lastName;
                    user.patronymic = request.patronymic ?? user.patronymic;
                    user.password = request.password ?? user.password;
                    user.role = request.role ?? user.role;

                    await ctx.SaveChangesAsync();
                    return "";
                }

                return String.Format("User ({0} does not exist", request.uuid);
            }
        }

        public async Task<UserModel> GetUser(UserModel request)
        {
            var user = new UserModel();
            using (ApplicationContext ctx = new ApplicationContext())
            {
                user = await ctx.users.FindAsync(request.uuid);
            }

            //I don't know what this is for 
            if (user.firstName == "unk")
            {
                return await Task.FromResult(new UserModel());

            }

            return await Task.FromResult(new UserModel()
            {
                uuid = request.uuid,
                email = request.email,
                firstName = request.firstName,
                lastName = request.lastName,
                patronymic = request.patronymic,
                password = request.password,
                role = request.role
            });

        }
    }
}
