using CompanyAboutDbService.Database;
using CompanyAboutDbService.Database.Models;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;

namespace CompanyAboutDbService.Managers
{
    public class CompanyAboutDbManager
    {
        public async Task<string> AddCompanyAbout(CompanyAboutModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult=="")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {

                        var state = await ctx.companyAbouts.AddAsync(new CompanyAboutModel()
                        {
                            uuid = Guid.NewGuid(),
                            title = request.title,
                            description = request.description,
                            executives = request.executives,
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added company successfully";
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

        public async Task<string> DeleteCompanyAboutById(CompanyAboutModel model)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var companyAbout = ctx.companyAbouts.Where(p => p.uuid == model.uuid).ToList()[0];
                if (companyAbout != null)
                {
                    ctx.companyAbouts.Remove(companyAbout);
                    await ctx.SaveChangesAsync();
                    return "";
                }

                return String.Format("Company ({0} does not exist", model.uuid);
            }
        }

        public async Task<string> ModifyCompanyAboutById(CompanyAboutModel request)
        {
            using (ApplicationContext ctx = new ApplicationContext())
            {
                var companyAbout = ctx.companyAbouts.Where(p => p.uuid == request.uuid).ToList()[0];
                if (companyAbout != null)
                {
                    companyAbout.title = request.title ?? companyAbout.title;
                    companyAbout.description = request.description ?? companyAbout.description;
                    companyAbout.executives = request.executives ?? companyAbout.executives;

                    await ctx.SaveChangesAsync();
                    return "";
                }

                return $"Company ({request.uuid} does not exist";
            }
        }

        public async Task<CompanyAboutModel> GetCompanyAboutById(CompanyAboutModel request)
        {
            var companyAbout = new CompanyAboutModel();
            using (ApplicationContext ctx = new ApplicationContext())
            {
                companyAbout = await ctx.companyAbouts.FindAsync(request.uuid);
            }

            if (companyAbout.title == null)
            {
                return await Task.FromResult(new CompanyAboutModel());
            }

            return await Task.FromResult(companyAbout);

        }
        
        private string Validate(CompanyAboutModel companyAbout)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(companyAbout);
            string errors = "";
            if (!Validator.TryValidateObject(companyAbout, context, results, true))
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
