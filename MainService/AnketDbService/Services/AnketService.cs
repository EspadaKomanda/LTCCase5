using AnketDbService.Database;
using AnketDbService.Database.Models;
using Grpc.Core;

namespace AnketDbService.Services
{
    public class AnketService : Anket.AnketBase
    {
        public async override Task<GetAnketReply> GetAnket(GetAnketRequest request, ServerCallContext context)
        {
            List<AnketModel> ankets = new List<AnketModel>();
            using (ApplicationContext ctx = new ApplicationContext())
            {
                ankets = ctx.ankets.ToList();
            }

            GetAnketReply reply = new GetAnketReply();
            reply.Anket.AddRange(await convertList(ankets));
            return reply;
        }

        private async Task<List<AnketPart>> convertList(List<AnketModel> ankets)
        {
            List<AnketPart> result = new List<AnketPart>();
            foreach (var VARIABLE in ankets)
            {
                var anket = new AnketPart()
                {
                    ParentId = VARIABLE.parentID.ToString(),
                    Text = VARIABLE.text,
                    Type = VARIABLE.type
                };
                anket.QuestionVariants.AddRange(VARIABLE.answerVariants.Split("-_-"));
                result.Add(anket);
            }  
            return result;
        }
    }
}
