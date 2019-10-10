using Prism.Commands;
using Prism.Navigation;
using SearchPeople.Models;
using SearchPeople.Services;
using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using static SearchPeople.Utils.MediaHelper;

namespace SearchPeople.ViewModels
{
    public class SearchPeoplePageViewModel : IInitialize, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PersonFace> TrainingTempImages { get; set; } = new ObservableCollection<PersonFace>();
        public ObservableCollection<Person> TrainingListImages { get; set; } = new ObservableCollection<Person>();

        public ImageSource MyImage
        {
            get;
            set;
        }
        public string PersonName { get; set; }

        public DelegateCommand AddFaceToPersonCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }

        public DelegateCommand SearchInPhotosCommand { get; set; }


        private readonly MediaHelper mediaHelper = new MediaHelper();


        private IRecognitionAppService _recognitionAppService;

        INavigationService _navigationService;

        public SearchPeoplePageViewModel(IRecognitionAppService recognitionAppService, INavigationService navigationService)
        {
            _recognitionAppService = recognitionAppService;
            _navigationService = navigationService;
        }


        public void Initialize(INavigationParameters parameters)
        {
            AddFaceToPersonCommand = new DelegateCommand(async () =>
            {
                var images = await mediaHelper.PickMultipleImages();
                if (images != null)
                {
                    foreach (var image in images)
                    {  
                        TrainingTempImages.Add(new PersonFace()
                        {
                            Image = image.Image,
                            Path = image.Path
                        });

                        MyImage = image.Image;
                    }
                }
            });


            AddPersonCommand = new DelegateCommand(async () =>
           {
               try
               {

                   List<PersonFace> tempImages = TrainingTempImages.ToList();

                   TrainingListImages.Add(new Person()
                   {
                       Name = PersonName,
                       TrainingTempImages = tempImages
                   });


                   var streamImages = tempImages.Select(d => new FileStream(d.Path, FileMode.Open));

                   await _recognitionAppService.CreatePerson(streamImages, PersonName);
               }
               catch (Exception ex)
               {

               }
           });


            SearchInPhotosCommand = new DelegateCommand(async () =>
            {
                var images = await mediaHelper.PickMultipleImages();
                if (images != null)
                {

                    List<ImagePhoto> imageFindend = new List<ImagePhoto>();

                    await _recognitionAppService.SearchPersonInPictures(images.Select(d => d.StreamImage), (stringPath, people) =>
                    {
                        var imageSource = ImageSource.FromFile(stringPath);

                        imageFindend.Add(new ImagePhoto()
                        {
                            Image = imageSource,
                            Path = stringPath,
                            Text = string.Join(",", people.Select(d => d.Name))
                        });
                    });
                    var findedImagesParameter = new NavigationParameters();
                    findedImagesParameter.Add("ImageList", imageFindend);
                    Debug.WriteLine(imageFindend.Count);
                    await _navigationService.NavigateAsync(new Uri(NavigationConstants.FindedImage, UriKind.Relative), findedImagesParameter);
                }
            });

        }
    }
}
