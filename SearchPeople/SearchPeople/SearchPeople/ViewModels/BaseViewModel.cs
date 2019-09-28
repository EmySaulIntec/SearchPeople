using Acr.UserDialogs;
using SearchPeople.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SearchPeople.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public IUserDialogs _pageDialog = UserDialogs.Instance;
        public IApiManager _apiManager;
        public IApiService<IAzureApi> _makeUpApi = new ApiService<IAzureApi>(config.ApiUrl);

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get; set; }

        public BaseViewModel()
        {
            _apiManager = new ApiManager(_makeUpApi);
        }

        public async Task RunSafe(Task task, bool ShowLoading = true, string loadingMessage = null)
        {
            try
            {
                if (IsBusy) return;

                IsBusy = true;

                if (ShowLoading) UserDialogs.Instance.ShowLoading(loadingMessage ?? "Loading");

                await task;
            }
            catch (Exception e)
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();

                await App.Current.MainPage.DisplayAlert("Problem with connection ", e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                if (ShowLoading) UserDialogs.Instance.HideLoading();
            }
        }
    }
}
