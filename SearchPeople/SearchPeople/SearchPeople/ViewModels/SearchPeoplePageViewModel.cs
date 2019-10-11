using Prism.Commands;
using Prism.Navigation;
using SearchPeople.Extends;
using SearchPeople.Models;
using SearchPeople.Services;
using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using static SearchPeople.Utils.MediaHelper;

namespace SearchPeople.ViewModels
{
    public class SearchPeoplePageViewModel : BaseViewModel, IInitialize, INavigationAware, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MultipleImage> ListedImages { get; set; } = new ObservableCollection<MultipleImage>();

        private ObservableCollection<Person> _trainingListImages;

        public ObservableCollection<Person> TrainingListImages
        {
            get { return _trainingListImages; }
            set
            {
                _trainingListImages = value;
                OnPropertyChanged(nameof(TrainingListImages));
            }
        }

        public string PersonName { get; set; }

        public DelegateCommand AddFaceToPersonCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }

        public DelegateCommand SearchInPhotosCommand { get; set; }
        public DelegateCommand<Person> DeleteCommand { get; set; }
        public DelegateCommand<FaceToDelete> DeleteFaceCommand { get; set; }
        public DelegateCommand LoadImagesCommand { get; set; }
        public DelegateCommand<Person> ViewImagesCommand { get; set; }
        public bool IsRegreshing { get; set; }
        public bool IsNotComparig { get; set; } = true;

        public bool IsNotRefreshing
        {
            get
            {
                return !IsRegreshing;
            }
        }

        private readonly MediaHelper mediaHelper = new MediaHelper();

        public static bool GroupCreated { get; set; }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private IRecognitionAppService _recognitionAppService;
        private INavigationService _navigationService;
        private INavigationParameters _parameters;
        public SearchPeoplePageViewModel(INavigationService navigationService)
        {

            _navigationService = navigationService;

            TrainingListImages = new ObservableCollection<Person>();

            AddPersonCommand = new DelegateCommand(async () =>
            {
                try
                {
                    IsRegreshing = true;
                    _recognitionAppService = new RecognitionAppService();

                    List<PersonFace> tempImages = new List<PersonFace>();

                    var images = await mediaHelper.PickMultipleImages();
                    if (images != null)
                    {
                        foreach (var image in images)
                        {
                            tempImages.Add(new PersonFace()
                            {
                                Image = image.Image,
                                Path = image.Path
                            });
                        }
                    }
                    else
                        return;

                    if (!GroupCreated)
                    {
                        GroupCreated = true;
                        await _recognitionAppService.CreateGroupAsync();
                    }

                    if (string.IsNullOrEmpty(PersonName))
                    {
                        return;
                    }


                    var streamImages = tempImages.Select(d => d.ImageStream).ToList();

                    var personId = await _recognitionAppService.CreatePerson(streamImages, PersonName);

                    TrainingListImages.Add(new Person()
                    {
                        Name = PersonName,
                        TrainingTempImages = tempImages,
                        PersonId = personId
                    });

                    PersonName = string.Empty;

                    IsRegreshing = false;
                }
                catch (Exception ex)
                {

                }
            });

            SearchInPhotosCommand = new DelegateCommand(async () =>
            {
                IsRegreshing = true;
                IsNotComparig = false;
                var images = await mediaHelper.PickMultipleImages();
                if (images != null)
                {

                    List<ImagePhoto> imageFindend = new List<ImagePhoto>();

                    await _recognitionAppService.SearchPersonInPictures(images.Select(d => d.StreamImage), (stringPath, people) =>
                    {

                        imageFindend.Add(new ImagePhoto()
                        {
                            Path = stringPath,
                            Text = string.Join(",", people.Select(d => d.Name))
                        });
                    });

                    ObservableCollection<PersonFace> imagesObserbleCollection = imageFindend.Select(e => new PersonFace()
                    {
                        Path = e.Path,
                        Image = e.Image,
                    }).ToObservableCollection();


                    _parameters.Add("images", imagesObserbleCollection);
                    _parameters.Add("name", "Searched Images");

                    await _navigationService.NavigateAsync(NavigationConstants.Gallery, _parameters);

                    IsRegreshing = false;
                    IsNotComparig = true;
                }
            });

            DeleteCommand = new DelegateCommand<Person>((person) =>
            {
                TrainingListImages.Remove(person);
                _recognitionAppService.DeletePerson(person.PersonId);
            });

            DeleteFaceCommand = new DelegateCommand<FaceToDelete>((face) =>
            {
                var indexPerson = TrainingListImages.IndexOf(face.Person);

                if (indexPerson >= 0)
                {
                    var personFace = TrainingListImages[indexPerson].TrainingTempImages.IndexOf(face.Face);


                    TrainingListImages[indexPerson].TrainingTempImages.RemoveAt(personFace);

                    OnPropertyChanged(nameof(TrainingListImages));
                }
            });

            ViewImagesCommand = new DelegateCommand<Person>(async (person) =>
            {
                if (person == null)
                    return;

                ObservableCollection<PersonFace> imagesObserbleCollection = person.TrainingTempImages.ToObservableCollection();

                _parameters.Add("images", imagesObserbleCollection);
                _parameters.Add("name", person.Name);
                _parameters.Add("delete", DeleteFaceCommand);
                _parameters.Add("item", person);

                await _navigationService.NavigateAsync(NavigationConstants.Gallery, _parameters);
            });
        }

        public void Initialize(INavigationParameters parameters)
        {
            _parameters = parameters;


        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _parameters = parameters;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
