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
            InitializeComponent();

            _recognitionAppService = new RecognitionAppService();
        }


        private async void BtnLoadTrainingImages_Clicked(object sender, EventArgs e)
        {
            var created = await _recognitionAppService.CreateGroupAsync();

            var trainingImages = await mediaHelper.PickMultipleImages();

            var s = trainingImages.FirstOrDefault().Path;

            var subFolders = Directory.GetDirectories(s);

            ListTrainingImages.ItemsSource = trainingImages;

            await _recognitionAppService.CreateGroupAsync();
            await _recognitionAppService.CreatePerson(trainingImages.Select(d => d.StreamImage), "A");

        }

        private async void BtnLoadTestImages_Clicked(object sender, EventArgs e)
        {
            var testImages = await mediaHelper.PickMultipleImages();
            ListTestImages.ItemsSource = testImages;

            List<ImagePhoto> imageFindend = new List<ImagePhoto>();

            await _recognitionAppService.SearchPersonInPictures(testImages.Select(d => d.StreamImage), (stringPath, people) =>
            {
                var imageSource = ImageSource.FromFile(stringPath);

                imageFindend.Add(new ImagePhoto()
                {
                    Path = stringPath,
                    Text = string.Join(",", people.Select(d => d.Name))
                });
            });

            ListImagePhotos.ItemsSource = imageFindend;

        }


    }
}