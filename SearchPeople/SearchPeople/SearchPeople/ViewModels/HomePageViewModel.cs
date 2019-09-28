using Newtonsoft.Json;
using Prism.Commands;
using SearchPeople.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
            var azureResponse = await _apiManager.GetAzureData();

            if (azureResponse.IsSuccessStatusCode)
            {
                var response = await azureResponse.Content.ReadAsStringAsync();
                IEnumerable<Group> jsonGroups = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Group>>(response));
             
                Groups = new ObservableCollection<Group>(jsonGroups);
            }else
            {
                await _pageDialog.AlertAsync("Problem getting data", "Error", "Ok");
            }
        }
    }
}
