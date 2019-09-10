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

        private static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            foreach (var num in Enumerable.Range(0, 10))
            {
                yield return num;
            }
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
