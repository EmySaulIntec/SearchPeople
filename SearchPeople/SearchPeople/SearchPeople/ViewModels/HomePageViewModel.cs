using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SearchPeople.Extends;
using SearchPeople.Models;
using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SearchPeople.ViewModels
{
    public class HomePageViewModel : IInitialize, INavigationAware, INotifyPropertyChanged
    {
        private IPageDialogService _pageDialogService;
        private IMonkeyManager _monkeyManager;
        private INavigationParameters _parameters;
        private INavigationService _navigationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public string BackImg { get; set; } = "ic_addyoursearch.png";

        public bool IsEmpty
        {
            get
            {
                return GroupFindeds.Count == 0;
            }
        }

        public bool IsNotEmpty
        {
            get
            {
                return GroupFindeds.Count > 0;
            }
        }
        public ObservableCollection<GroupFinded> GroupFindeds { get; set; } = new ObservableCollection<GroupFinded>();

        public DelegateCommand<GroupFinded> ViewPeopleCommand { get; set; }
        public DelegateCommand<GroupFinded> ViewFindedCommand { get; set; }

        public DelegateCommand CallSearchedPageCommand { get; set; }

        public DelegateCommand<GroupFinded> DeleteCommand { get; set; }
        public HomePageViewModel(IPageDialogService pageDialogService, IMonkeyManager monkeyManager, INavigationService navigationService)
        {
            _pageDialogService = pageDialogService;
            _monkeyManager = monkeyManager;
            _navigationService = navigationService;

            var monkey = _monkeyManager.GetMonkey<List<GroupFinded>>(Constants.GROUPED_FINDED);
            if (monkey != null)
            {
                GroupFindeds = monkey.Item.ToObservableCollection();
            }

            ViewPeopleCommand = new DelegateCommand<GroupFinded>(async (people) =>
            {
                await CallGallery(people.PeopleName, people.PeoplePhotos);
            });

            ViewFindedCommand = new DelegateCommand<GroupFinded>(async (people) =>
            {
                await CallGallery(Constants.MAIN_TITLE, people.SearchedPeople);
            });

            CallSearchedPageCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(NavigationConstants.SearchPeople);
            });

            DeleteCommand = new DelegateCommand<GroupFinded>((people) =>
           {
               GroupFindeds.Remove(people);
               _monkeyManager.SaveMokey<List<GroupFinded>>(GroupFindeds.ToList(), Constants.GROUPED_FINDED);
           });
        }

        private async Task CallGallery(string name, IEnumerable<string> paths)
        {
            var images = paths.Select(e => new PersonFace()
            {
                Path = e
            }).ToObservableCollection();

            _parameters.Add(Constants.GALLERY_IMAGES, images);
            _parameters.Add(Constants.GALLERY_NAME, name);

            await _navigationService.NavigateAsync(NavigationConstants.Gallery, _parameters);
        }

        public HomePageViewModel()
        {
        }

        public void Initialize(INavigationParameters parameters)
        {
            _parameters = parameters;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _parameters = parameters;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
