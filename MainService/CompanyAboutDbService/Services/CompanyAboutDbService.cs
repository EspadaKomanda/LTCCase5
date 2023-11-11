using CompanyAboutDbService.Database.Models;
using CompanyAboutDbService.Managers;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;

namespace CompanyAboutDbService.Services
{
    public class CompanyAboutDbService : CompanyAboutDb.CompanyAboutDbBase
    {
        private CompanyAboutDbManager _manager = new CompanyAboutDbManager();
        public async override Task<CreateCompanyAboutReply> CreateCompanyAbout(CreateCompanyAboutRequest request, ServerCallContext context)
        {
            var result = await _manager.AddCompanyAbout(new CompanyAboutModel()
            {
                uuid = Guid.NewGuid(),
                title = request.Title,
                description = request.Description,
                executives = request.Executives
            });
            return await Task.FromResult(new CreateCompanyAboutReply()
            {
                Result = result
            });
        }

        public async override Task<DeleteCompanyAboutByIdReply> DeleteCompanyAboutById(DeleteCompanyAboutByIdRequest request, ServerCallContext context)
        {
            Guid companyAboutId;
            Guid.TryParse(request.Uuid, out companyAboutId);
            var result = await _manager.DeleteCompanyAboutById(new CompanyAboutModel()
            {
                uuid = companyAboutId
            });
            return await Task.FromResult(new DeleteCompanyAboutByIdReply()
            {
                Reply = result
            });
        }

        public async override Task<ModifyCompanyAboutByIdReply> ModifyCompanyAboutById(ModifyCompanyAboutByIdRequest request, ServerCallContext context)
        {
            Guid companyAboutId;
            Guid.TryParse(request.Uuid, out companyAboutId);
            var result = await _manager.ModifyCompanyAboutById(new CompanyAboutModel()
            {
                title = request.Title,
                description = request.Description,
                executives = request.Executives,
                uuid = companyAboutId
            });
            return await Task.FromResult(new ModifyCompanyAboutByIdReply()
            {
                Reply = result
            });
        }

        public async override Task<GetReply> GetCompanyAboutById(GetCompanyAboutByIdRequest request, ServerCallContext context)
        {
            //Сделал тут TryParse для Guid как в примерах выше.
            //Вчера был слишком сонный для этого.
            Guid companyAboutId;
            Guid.TryParse(request.Uuid, out companyAboutId);
            var result = await _manager.GetCompanyAboutById(new CompanyAboutModel()
            {
                uuid = companyAboutId
            });
            return await Task.FromResult(new GetReply()
            {
                Title = result.title,
                Description = result.description,
                Executives = result.executives,
                Uuid = result.uuid.ToString()
            });
        }
    }
}
