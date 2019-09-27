using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SearchPeople.ViewModels
{
    public class HomeMasterDetailPageViewModel : IInitialize
    {

        private readonly INavigationService _navigationService;
        public DelegateCommand<string> GoTo { get; set; }

        public HomeMasterDetailPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            GoTo = new DelegateCommand<string>(async (page) =>
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
