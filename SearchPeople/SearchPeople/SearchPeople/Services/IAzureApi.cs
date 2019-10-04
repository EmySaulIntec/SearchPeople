using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    [Headers("Content-Type: application/json")]
    public interface IAzureApi
    {
       
        [Get("/persongroups?top=1000&returnRecognitionModel=false")]
        Task<HttpResponseMessage> GetGroups([Header(Config.NameAzureKey)] string key);

    }
}
