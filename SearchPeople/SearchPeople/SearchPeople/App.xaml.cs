using Xamarin.Forms;
using Prism;
using Prism.Unity;
using Prism.Ioc;
using SearchPeople.Views;
using SearchPeople.ViewModels;
using SearchPeople.Services;
using SearchPeople.TestPlugins;
using SearchPeople.TestPlugins.ViewModels;
using SearchPeople.Utils;
using MonkeyCache.FileStore;

namespace SearchPeople
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
            Barrel.ApplicationId = Constants.BARREL_ID;
        }


        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync(NavigationConstants.Home);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<AboutUsPage, AboutUsPageViewModel>();
            containerRegistry.RegisterForNavigation<ConfigurationPage, ConfigurationPageViewModel>();
            containerRegistry.RegisterForNavigation<DetailPage, DetailPageViewModel>();
            containerRegistry.RegisterForNavigation<FindedImagePage, FindedImagePageViewModel>();
            containerRegistry.RegisterForNavigation<HomeMasterDetailPage, HomeMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<SearchPeoplePage, SearchPeoplePageViewModel>();
            containerRegistry.RegisterForNavigation<GalleryPage, GalleryPageViewModel>();


            containerRegistry.RegisterInstance<IMediaHelper>(new MediaHelper());
            containerRegistry.RegisterInstance<IMonkeyManager>(new MonkeyManager());


        }

    }
}
