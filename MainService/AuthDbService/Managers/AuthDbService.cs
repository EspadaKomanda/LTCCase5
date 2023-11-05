using AuthDbService.Database;
using AuthDbService.Database.Models;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;

namespace AuthDbService.Managers
{
    public class AuthDbService
    {
        public async Task<string> AddUser(UserModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult=="")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {

                        var state = await ctx.users.AddAsync(new UserModel()
                        {
                            uuid = Guid.NewGuid(),
                            email = request.email,
                            firstName = request.firstName,
                            lastName = request.lastName,
                            patronymic = request.patronymic,
                            password = request.password,
                            role = request.role
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added user successfully";
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
            else
            {
                return ValidationResult;
            }
          
        }

        public async Task<string> DeleteUserByEmail(UserModel model)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var user =  ctx.users.Where(p => p.email == model.email).ToList()[0];
                if (user != null)
                {
                    ctx.users.Remove(user);
                    await ctx.SaveChangesAsync();
                    return "";
                }

                return String.Format("User ({0} does not exist", model.email);
            }
        }

        public async Task<string> ModifyUserByEmail(UserModel request)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var user = ctx.users.Where(p => p.email == request.email).ToList()[0];
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

                return $"User ({request.email} does not exist";
            }
        }
        public async Task<string> DeleteUserById(UserModel model)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var user = await ctx.users.FindAsync(model.uuid);
                if (user != null)
                {
                    ctx.users.Remove(user);
                    await ctx.SaveChangesAsync();
                    return "";
                }

                return String.Format("User ({0} does not exist", model.email);
            }
        }

        public async Task<string> ModifyUserById(UserModel request)
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

                return $"User ({request.email} does not exist";
            }
        }
        public async Task<UserModel> GetUserById(UserModel request)
        {
            var user = new UserModel();
            using (ApplicationContext ctx = new ApplicationContext())
            {
                user = await ctx.users.FindAsync(request.uuid);
            }


            if (user.firstName == null)
            {
                return await Task.FromResult(new UserModel());
            }

            return await Task.FromResult(user);

        }
        public async Task<UserModel> GetUserByEmail(UserModel request)
        {
            var user = new UserModel();
            using (ApplicationContext ctx = new ApplicationContext())
            {
                user = await ctx.users.FindAsync(request.uuid);
            }

            if (user.firstName == null)
            {
                return await Task.FromResult(new UserModel());
            }

            return await Task.FromResult(user);

        }
        private string Validate(UserModel user)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);
            string errors = "";
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                foreach (var error in results)
                {
                    errors+=error.ErrorMessage+'\n';
                }
            }

            return errors;
        }

    }
}
