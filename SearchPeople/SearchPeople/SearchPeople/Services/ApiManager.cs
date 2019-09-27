using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    public class ApiManager : IApiManager
    {
        IUserDialogs _userDialogs = UserDialogs.Instance;
        IConnectivity _connectivity = CrossConnectivity.Current;
        IApiService<IMakeUpApi> _makeUpApi;

        public bool IsConnected { get; set; }
        public bool IsReachable { get; set; }

        Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();

        public ApiManager(IApiService<IMakeUpApi> makeUpApi)
        {
            _makeUpApi = makeUpApi;

            IsConnected = _connectivity.IsConnected;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.IsConnected;

            if (!e.IsConnected)
            {
                // Cancell All Running Task
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task) where TData : HttpResponseMessage, new()
        {
            TData data = new TData();

            if (!IsConnected)
            {
                var strngResponse = "There's not a network connection";

                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                _userDialogs.Toast(strngResponse, TimeSpan.FromSeconds(1));

                return data;
            }



            //IsReachable = await _connectivity.IsRemoteReachable(config.ApiHostName);
            IsReachable = await _connectivity.IsRemoteReachable(config.ApiUrl,TimeSpan.FromSeconds(1));

            if (!IsReachable)
            {
                var strgResponse = "There's not an internet connection";

                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strgResponse);

                _userDialogs.Toast(strgResponse, TimeSpan.FromSeconds(1));
                return data;
            }

            data = await Policy
                .Handle<WebException>()
                .Or<ApiException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync
                (
                 retryCount: 1,
                 sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                ).ExecuteAsync(async () =>
                {
                    var result = await task;
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Logout the user
                    }
                    return result;
                });
            return data;
        }

        public async Task<HttpResponseMessage> GetMakeUps(string brand)
        {
            var cts = new CancellationTokenSource();
            var api = _makeUpApi.GetApi(Fusillade.Priority.UserInitiated);
            var data = api.GetMakeUps(brand);

            var task = RemoteRequestAsync<HttpResponseMessage>(data);
            runningTasks.Add(task.Id, cts);

            return await task;
        }
    }
}
