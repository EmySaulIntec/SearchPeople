using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using static SearchPeople.Utils.MediaHelper;

namespace SearchPeople.ViewModels
{
    public class FindedImagePageViewModel : IInitialize, INavigatedAware
    {
        INavigationService _navigationService;
        private DelegateCommand<ImagePhoto> _selectImage;
        public DelegateCommand<ImagePhoto> SelectImage => _selectImage ?? (_selectImage = new DelegateCommand<ImagePhoto>(ToDetailPage));
        public List<ImagePhoto> ImageList { get; set; }
        private async void ToDetailPage(ImagePhoto imageToDetail)
        {
            var parameter = new NavigationParameters();
            parameter.Add("ImageToDetail", imageToDetail);
            await _navigationService.NavigateAsync(new Uri(NavigationConstants.Detail, UriKind.Relative), parameter);
        }
        public FindedImagePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public void Initialize(INavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            List<ImagePhoto> imageList = (List<ImagePhoto>)parameters["ImageList"];
            ImageList = imageList;
        }
    }
}
