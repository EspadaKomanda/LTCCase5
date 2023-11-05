
using ConfigService.Configs;
using Grpc.Core;

namespace ConfigService.Services;

public class ConfigService : Config.ConfigBase
{
    public async override Task<DbReply> GetDb(DbRequest request, ServerCallContext context)
    {
        var dbInfo = await ConfigReader.GetDb(request.Name);
        return await Task.FromResult(new DbReply()
        {
            Databasename = dbInfo.DatabaseName,
            Password = dbInfo.Password,
            Server = dbInfo.Server,
            User = dbInfo.User
        });
    }

    public  async override Task<UrlReply> GetUrl(UrlRequest request, ServerCallContext context)
    {
        var url = await ConfigReader.GetUrl(request.UrlName);
        Console.WriteLine(url.Url);
        var x = url.Url.ToString();
        return await Task.FromResult(new UrlReply()
        {
            Url = x
        });
    }

    public async override Task<ClientReply> GetClientInfo(ClientRequest request, ServerCallContext context)
    {
        var client = await ConfigReader.GetClient();
        return await Task.FromResult(new ClientReply()
        {
            ClientId = client.ClietnId,
            ClientSecret = client.ClientSecret
        });
    }

    public async override Task<AReply> GetAuth(ARequest request, ServerCallContext context)
    {
        var Auth = await ConfigReader.GetAuth();
        return await Task.FromResult(new AReply()
        {
            AUDIENCE = Auth.AUDIENCE,
            ISSUER = Auth.ISSUER,
            KEY = Auth.KEY
        });
    }
}