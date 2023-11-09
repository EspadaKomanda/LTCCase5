using StudyModulesDbService.Database;
using StudyModulesDbService.Database.Models;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;

namespace StudyModulesDbService.Managers
{
    public class StudyModulesDbManager
    {

        public async Task<string> AddStudyModule(StudyModuleModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var state = await ctx.studyModules.AddAsync(new StudyModuleModel()
                        {
                            uuid = Guid.NewGuid(),
                            name = request.name,
                            asignee = request.asignee
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added big module successfully";
                        }
                        return "EntityState.Added is false";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        private string Validate<T>(T t_model)
        {
            try
            {
                if (t_model == null)
                {
                    throw new ArgumentNullException(nameof(t_model));
                }

                var results = new List<ValidationResult>();
                var context = new ValidationContext(t_model);
                string errors = "";
                if (!Validator.TryValidateObject(t_model, context, results, true))
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
