using MonkeyCache.FileStore;
using SearchPeople.TestPlugins;
using Xamarin.Forms;
using Prism;
using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;

namespace SearchPeople
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        { }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialized()
        {
            throw new System.NotImplementedException();
        }
    }
}
