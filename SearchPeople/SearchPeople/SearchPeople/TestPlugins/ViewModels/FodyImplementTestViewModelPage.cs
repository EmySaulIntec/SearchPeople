using SearchPeople.Utils;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SearchPeople.TestPlugins.ViewModels
{
    public class FodyImplementTestViewModelPage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ImagePath { get; set; }
        public ImageSource ImageSource { get; set; }

        public ICommand TakePhotoCommand { get; set; }

        private readonly MediaHelper mediaHelper = new MediaHelper();
        public FodyImplementTestViewModelPage()
        {
            TakePhotoCommand = new Command(async () =>
           {
               MediaHelper.ImagePhoto result = await mediaHelper.TakePhotoAsync();
               if (result != null)
               {
                   ImageSource = result.Image;
                   ImagePath = result.Path;
               }
           });
        }
    }
}
