using Refit;
using SearchPeople.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    [Headers("Content-Type: application/json")]
    public interface IAzureApi
    {
       
        [Get("/persongroups?top=1000&returnRecognitionModel=false")]
        Task<HttpResponseMessage> GetGroups([Header(config.NameAzureKey)] string key);

    }
}
