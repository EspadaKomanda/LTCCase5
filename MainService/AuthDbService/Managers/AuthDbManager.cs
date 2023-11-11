using AuthDbService.Database;
using AuthDbService.Database.Models;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AuthDbService.Managers
{
    public class AuthDbManager
    {
        public async Task<List<UserModel>> GetUsers()
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    return ctx.users.ToList();
                }
            }
            catch(Exception ex) 
            {

                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<string> AddUser(UserModel request)
        {
            string ValidationResult = Validate(request);
            
                try
                {

                    Console.WriteLine(request.dateOfBirth);
                    Console.WriteLine("Creating User!!!!");
                    using (ApplicationContext ctx = new ApplicationContext())
                    {

                       await ctx.users.AddAsync(new UserModel()
                        {
                            uuid = Guid.NewGuid(),
                            email = request.email,
                            phone = request.phone,
                            telegram = request.telegram,
                            firstName = request.firstName,
                            lastName = request.lastName,
                            patronymic = request.patronymic,
                            password = request.password,
                            position = request.position,
                            role = request.role,
                            about = request.about,
                            avatar = request.avatar,
                            dateOfBirth = request.dateOfBirth
                        });

                        await ctx.SaveChangesAsync();
                    Console.WriteLine("Created User!!!!");
                    return "Added user successfully";
                     
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
          

        }

        public async Task<string> DeleteUserByEmail(UserModel model)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var user = ctx.users.Where(p => p.email == model.email).ToList()[0];
                    if (user != null)
                    {
                        ctx.users.Remove(user);
                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return String.Format("User ({0} does not exist", model.email);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.ToString();
            }
          
        }

        public async Task<string> ModifyUserByEmail(UserModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var user = ctx.users.Where(p => p.email == request.email).ToList()[0];
                    if (user != null)
                    {
                        user.email = request.email ?? user.email;
                        user.phone = request.phone ?? user.phone;
                        user.telegram = request.telegram ?? user.telegram;
                        user.firstName = request.firstName ?? user.firstName;
                        user.lastName = request.lastName ?? user.lastName;
                        user.patronymic = request.patronymic ?? user.patronymic;
                        user.password = request.password ?? user.password;
                        user.position = request.position ?? user.position;
                        user.role = request.role ?? user.role;
                        user.about = request.about ?? user.about;
                        user.avatar = request.avatar ?? user.avatar;

                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return $"User ({request.email} does not exist";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
          
        }
        public async Task<string> DeleteUserById(UserModel model)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }

        public async Task<string> ModifyUserById(UserModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var user = await ctx.users.FindAsync(request.uuid);
                    if (user != null)
                    {
                        user.email = request.email ?? user.email;
                        user.phone = request.phone ?? user.phone;
                        user.telegram = request.telegram ?? user.telegram;
                        user.firstName = request.firstName ?? user.firstName;
                        user.lastName = request.lastName ?? user.lastName;
                        user.patronymic = request.patronymic ?? user.patronymic;
                        user.password = request.password ?? user.password;
                        user.position = request.position ?? user.position;
                        user.role = request.role ?? user.role;
                        user.about = request.about ?? user.about;
                        user.avatar = request.avatar ?? user.avatar;

                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return $"User ({request.email} does not exist";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
           
        }
        public async Task<UserModel> GetUserById(UserModel request)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UserModel();
            }
        }
        public async Task<UserModel> GetUserByEmail(UserModel request)
        {
            try
            {
                var user = new UserModel();
                Console.WriteLine(request.email);
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    user = ctx.users.Where(p => p.email == request.email).ToList()[0];
                }

                if (user.firstName == null)
                {
                    return await Task.FromResult(new UserModel());
                }

                return await Task.FromResult(user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UserModel();
            }

        }
        private string Validate(UserModel user)
        {
            try
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(user);
                string errors = "";
                if (!Validator.TryValidateObject(user, context, results, true))
                {
                    foreach (var error in results)
                    {
                        errors += error.ErrorMessage + '\n';
                    }
                }

                return errors;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
          
        }

    }
}
