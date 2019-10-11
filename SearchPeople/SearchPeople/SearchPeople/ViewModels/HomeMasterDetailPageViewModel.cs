using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace SearchPeople.ViewModels
{
    public class HomeMasterDetailPageViewModel : IInitialize
    {
        private readonly INavigationService _navigationService;
        public DelegateCommand<string> GoToPageCommand { get; set; }

        public HomeMasterDetailPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            GoToPageCommand = new DelegateCommand<string>(async (page) =>
            {
                await GoToPage(page);
            });
        }

        private async Task GoToPage(string page)
        {
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }

        public void Initialize(INavigationParameters parameters)
        {
        }

    }
}
