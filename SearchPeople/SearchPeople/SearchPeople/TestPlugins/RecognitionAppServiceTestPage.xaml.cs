using SearchPeople.Services;
using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SearchPeople.Utils.MediaHelper;

namespace SearchPeople.TestPlugins
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionAppServiceTestPage : ContentPage
    {
        private readonly IRecognitionAppService _recognitionAppService;
        private readonly MediaHelper mediaHelper = new MediaHelper();

        public RecognitionAppServiceTestPage()
        {
            _recognitionAppService = new RecognitionAppService();
            _recognitionAppService.CreateGroupAsync();
            InitializeComponent();
        }

        private void BtnOpenDirectory_Clicked(object sender, EventArgs e)
        {
            var path = DependencyService.Get<IDirectoryDependency>().OpenDirectory();
            var subFolders = Directory.GetDirectories(path);
            var directories = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

        }

        private async void BtnLoadTrainingImages_Clicked(object sender, EventArgs e)
        {

            var trainingImages = await mediaHelper.PickMultiplePhotosAsync();

            var s = trainingImages.FirstOrDefault().Path;

            var subFolders = Directory.GetDirectories(s);

            ListTrainingImages.ItemsSource = trainingImages;

            await _recognitionAppService.CreateGroupAsync();
            await _recognitionAppService.CreatePerson(trainingImages.Select(d => d.StreamImage), "A");

        }

        private async void BtnLoadTestImages_Clicked(object sender, EventArgs e)
        {
            var testImages = await mediaHelper.PickMultiplePhotosAsync();
            ListTestImages.ItemsSource = testImages;

            List<ImagePhoto> imageFindend = new List<ImagePhoto>();

            await _recognitionAppService.SearchPersonInPictures(testImages.Select(d => d.StreamImage), (stringPath, people) =>
             {
                 var imageSource = ImageSource.FromFile(stringPath);

                 imageFindend.Add(new ImagePhoto()
                 {
                     Image = imageSource,
                     Path = stringPath,
                     Text = string.Join(",", people.Select(d => d.Name))
                 });
             });

            ListImagePhotos.ItemsSource = imageFindend;

        }


    }
}