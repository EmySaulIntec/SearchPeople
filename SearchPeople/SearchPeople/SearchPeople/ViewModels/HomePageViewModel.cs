using Acr.UserDialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Refit;
using SearchPeople.Models;
using SearchPeople.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SearchPeople.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public ObservableCollection<Group> Groups { get; set; }
        public DelegateCommand GetDataCommand { get; set; }

        public HomePageViewModel()
        {
            GetDataCommand = new DelegateCommand(async() => await RunSafe(GetData()));
        }

        private async Task GetData()
        {
            var makeUpsResponse = await _apiManager.GetAzureData();

            if (makeUpsResponse.IsSuccessStatusCode)
            {
                var response = await makeUpsResponse.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<Group>>(response));
             
                Groups = new ObservableCollection<Group>(json);
            }else
            {
                await _pageDialog.AlertAsync("Unable to get data", "Error", "Ok");
            }
        }
    }
}
