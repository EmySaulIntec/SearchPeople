using SearchPeople.TestPlugins.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SearchPeople.TestPlugins
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageWitPathPage : ContentPage
    {
        public ImageWitPathPage()
        {
            InitializeComponent();
            this.BindingContext = new FodyImplementTestViewModelPage();
        }
    }
}