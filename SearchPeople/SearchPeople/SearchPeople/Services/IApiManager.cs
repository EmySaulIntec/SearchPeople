using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> GetMakeUps(string brand);
    }
}
