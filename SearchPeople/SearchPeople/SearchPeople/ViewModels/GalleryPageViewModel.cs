using Prism.Commands;
using Prism.Navigation;
using SearchPeople.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SearchPeople.ViewModels
{
    public class GalleryPageViewModel : IInitialize, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PersonFace> ListedImages { get; set; } = new ObservableCollection<PersonFace>();
        public DelegateCommand<PersonFace> ViewImageCommand { get; set; }

        public string Name { get; set; }


        private PersonFace _selectedImage;
        public PersonFace SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
            }
        }

        public bool IsSelectedImage
        {
            get
            {
                return SelectedImage != null;
            }
        }
        public DelegateCommand DeleteCommand { get; set; }

        public GalleryPageViewModel()
        {
            ViewImageCommand = new DelegateCommand<PersonFace>((image) =>
            {
                SelectedImage = image;
            });
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("images"))
                ListedImages = (ObservableCollection<PersonFace>)parameters["images"];

            if (parameters.ContainsKey("name"))
                Name = (string)parameters["name"];

            if (parameters.ContainsKey("delete"))
            {
                var person = (Person)parameters["item"];
                var methodCommand = (DelegateCommand<FaceToDelete>)parameters["delete"];

                DeleteCommand = new DelegateCommand(() =>
               {
                   FaceToDelete face = new FaceToDelete()
                   {
                       Face = SelectedImage,
                       Person = person
                   };

                   methodCommand.Execute(face);

                   ListedImages.Remove(SelectedImage);
                   SelectedImage = null;
               });

            }

        }
    }
}
