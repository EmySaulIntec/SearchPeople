using Prism.Commands;
using Prism.Navigation;
using SearchPeople.Models;
using SearchPeople.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

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
                            Image = image.Image
                        });
                        MyImage = image.Image;
                    }
                }
            });


            AddPersonCommand = new DelegateCommand(() =>
           {
               List<PersonFace> tempImages = TrainingTempImages.ToList();

               TrainingListImages.Add(new Person()
               {
                   Name = PersonName,
                   TrainingTempImages = tempImages
               });
           });

            SearchInPhotosCommand = new DelegateCommand(async () =>
            {
                var images = await mediaHelper.PickMultipleImages();
                if (images != null)
                {
                    
                }
            });

        }
    }
}
