using System.Net.Http;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> GetAzureData();
    }
}
