using Fusillade;

namespace SearchPeople.Services
{
    public  interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
