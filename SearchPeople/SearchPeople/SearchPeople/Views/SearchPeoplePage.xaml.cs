using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SearchPeople.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPeoplePage : ContentPage
    {
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        public SearchPeoplePage()
        {
            InitializeComponent();

            imageSources.Add("Bolls.png");
            imageSources.Add("personCircle.png");

            imgSlider.Images = imageSources;
        }

    }
}