using SearchPeople.Utils;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SearchPeople.TestPlugins
{


    /// <summary>
    /// THIS CLASS IN THE FINAL PROJECT WILL ERASED
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileStoreTestPluginPage : ContentPage
    {
        private readonly MonkeyManager monkeyManager = new MonkeyManager();

        public FileStoreTestPluginPage()
        {
            InitializeComponent();

        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            string messageToSaved = txtInput.Text;
            string nameToItem = "message";

            monkeyManager.SaveMokey(messageToSaved, nameToItem);
            await DisplayAlert("File Store", "Saved", "Ok");
        }

        private void BtnGet_Clicked(object sender, EventArgs e)
        {
            string nameToItem = "message";
            var configurationItem = monkeyManager.GetMonkey<string>(nameToItem);
            txtInput.Text = configurationItem.Item;
        }
    }
}