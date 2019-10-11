using Prism.Commands;
using Prism.Navigation;
using System;
using SearchPeople.Models;
using System.Collections.ObjectModel;

namespace SearchPeople.ViewModels
{
    public class HomeMasterDetailPageViewModel : IInitialize
    {
        private DelegateCommand<MenuItemText> _goToPageCommand;
        public ObservableCollection<MenuItemText> TextList { get; set; } = new ObservableCollection<MenuItemText>();
        private readonly INavigationService _navigationService;

        public DelegateCommand<MenuItemText> GoToPageCommand => _goToPageCommand ?? (_goToPageCommand = new DelegateCommand<MenuItemText>(GoToPage));

        public HomeMasterDetailPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            TextList.Add(new MenuItemText("Home"));
            TextList.Add(new MenuItemText("SearchPeople"));
            TextList.Add(new MenuItemText("AboutUs"));

        }
        private async void GoToPage(MenuItemText paramData)
        {
            string page = paramData.Tittle;
            page = "HomeMasterDetailPage/NavigationPage/" + page + "Page";
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
        public void Initialize(INavigationParameters parameters)
        {
        }

    }
}
