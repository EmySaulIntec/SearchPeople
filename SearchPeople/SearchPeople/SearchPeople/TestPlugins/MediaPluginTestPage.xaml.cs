using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SearchPeople.TestPlugins
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPluginTestPage : ContentPage
    {
        private readonly MediaHelper mediaHelper = new MediaHelper();

        public MediaPluginTestPage()
        {
            InitializeComponent();
        }

        private async void BtnLoadImage_Clicked(object sender, EventArgs e)
        {
            MediaHelper.ImagePhoto result = await mediaHelper.TakePhotoAsync();
            if (result != null)
                loadedImage.Source = result.Image;
        }

        private async void BtnTakePhot_Clicked(object sender, EventArgs e)
        {
            MediaHelper.ImagePhoto result = await mediaHelper.TakePhotoAsync(true);
            if (result != null)
                loadedImage.Source = result.Image;
        }
    }
}