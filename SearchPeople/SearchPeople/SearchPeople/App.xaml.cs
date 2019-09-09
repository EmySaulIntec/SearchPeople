using MonkeyCache.FileStore;
using SearchPeople.TestPlugins;
using Xamarin.Forms;

namespace SearchPeople
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Barrel.ApplicationId = "SEARCH_PEOPLE_BARREL_ID";
            MainPage = new ImageWitPathPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
