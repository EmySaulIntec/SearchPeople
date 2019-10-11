using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SearchPeople.Extends;
using SearchPeople.Models;
using SearchPeople.Services;
using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;
using static SearchPeople.Utils.MediaHelper;

namespace SearchPeople.ViewModels
{
    public class SearchPeoplePageViewModel : BaseViewModel, IInitialize, INavigationAware, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IRecognitionAppService _recognitionAppService;
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;
        private INavigationParameters _parameters;
        private IMediaHelper _mediaHelper;

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

        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand SearchInPhotosCommand { get; set; }
        public DelegateCommand LoadImagesCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand<Person> DeleteCommand { get; set; }
        public DelegateCommand<FaceToDelete> DeleteFaceCommand { get; set; }
        public DelegateCommand<Person> ViewImagesCommand { get; set; }

        public string PersonName { get; set; }
        public bool IsRegreshing { get; set; }
        public bool IsNotComparig { get; set; } = true;
        public bool IsNotRefreshing
        {
            get
            {
                return !IsRegreshing;
            }
        }
        public static bool GroupCreated { get; set; }

        public SearchPeoplePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IMediaHelper mediaHelper)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _mediaHelper = mediaHelper;

            TrainingListImages = new ObservableCollection<Person>();

            AddPersonCommand = new DelegateCommand(async () =>
            {
                await AddPerson(mediaHelper);
            });

            SearchInPhotosCommand = new DelegateCommand(async () =>
            {
                await SearchInPhotos(mediaHelper);
            });

            DeleteCommand = new DelegateCommand<Person>((person) =>
            {
                DeletePerson(person);
            });

            DeleteFaceCommand = new DelegateCommand<FaceToDelete>((face) =>
            {
                DeleteFace(face);
            });

            ViewImagesCommand = new DelegateCommand<Person>(async (person) =>
            {
                await ViewImages(person);
            });

            CancelCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(NavigationConstants.Home);
            });

        }



        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
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


        private async System.Threading.Tasks.Task ViewImages(Person person)
        {
            if (person == null)
                return;

            ObservableCollection<PersonFace> imagesObserbleCollection = person.TrainingTempImages.ToObservableCollection();

            _parameters.Add(Constants.GALLERY_IMAGES, imagesObserbleCollection);
            _parameters.Add(Constants.GALLERY_NAME, person.Name);
            _parameters.Add(Constants.GALLERY_DELETE_METHOD, DeleteFaceCommand);
            _parameters.Add(Constants.GALLERY_ITEM, person);

            await _navigationService.NavigateAsync(NavigationConstants.Gallery, _parameters);
        }

        private void DeleteFace(FaceToDelete face)
        {
            var indexPerson = TrainingListImages.IndexOf(face.Person);

            if (indexPerson >= 0)
            {
                var personFace = TrainingListImages[indexPerson].TrainingTempImages.IndexOf(face.Face);

                TrainingListImages[indexPerson].TrainingTempImages.RemoveAt(personFace);

                OnPropertyChanged(nameof(TrainingListImages));
            }
        }

        private void DeletePerson(Person person)
        {
            TrainingListImages.Remove(person);
            _recognitionAppService.DeletePerson(person.PersonId);
        }

        private async System.Threading.Tasks.Task SearchInPhotos(IMediaHelper mediaHelper)
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
                        Text = string.Join(", ", people.Select(d => d.Name))
                    });
                });

                ObservableCollection<PersonFace> imagesObserbleCollection = imageFindend.Select(e => new PersonFace()
                {
                    Path = e.Path,
                }).ToObservableCollection();

                string allName = string.Join(", ", TrainingListImages.Select(d => d.Name));

                var allPath = TrainingListImages.SelectMany(e => e.TrainingTempImages).Select(d => d.Path).ToList();

                var searchedPeople = imageFindend.Select(e => e.Path).ToList();

                _parameters.Add(Constants.GALLERY_IMAGES, imagesObserbleCollection);
                _parameters.Add(Constants.GALLERY_NAME, Constants.MAIN_TITLE);
                _parameters.Add(Constants.GROUPED_FINDED, new GroupFinded()
                {
                    PeopleName = allName,
                    PeoplePhotos = allPath,
                    SearchedPeople = searchedPeople
                });

                await _navigationService.NavigateAsync(NavigationConstants.GallerySave, _parameters);

                IsRegreshing = false;
                IsNotComparig = true;
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Alert", Constants.IMAGE_NOT_LOADED, "Ok");
                IsRegreshing = false;
                IsNotComparig = true;
                return;
            }
        }

        private async System.Threading.Tasks.Task AddPerson(IMediaHelper mediaHelper)
        {
            try
            {
                var current = Connectivity.NetworkAccess;

                if (current != NetworkAccess.Internet)
                {
                    await _pageDialogService.DisplayAlertAsync("Alert", Constants.NOT_INTERNET, "Ok");
                    IsRegreshing = false;
                    return;
                }

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
                            Path = image.Path
                        });
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Alert", Constants.IMAGE_NOT_LOADED, "Ok");
                    IsRegreshing = false;
                    return;
                }

                if (!GroupCreated)
                {
                    GroupCreated = true;
                    var created = await _recognitionAppService.CreateGroupAsync();

                    if (!created)
                    {
                        await _pageDialogService.DisplayAlertAsync("Internal Error", Constants.GROUP_NOT_CREATED, "Ok");
                        IsRegreshing = false;
                        return;
                    }
                }

                if (string.IsNullOrEmpty(PersonName))
                {
                    await _pageDialogService.DisplayAlertAsync("Alert", Constants.PERSON_NAME_NOT_VALID, "Ok");
                    IsRegreshing = false;
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
                IsRegreshing = false;
                await _pageDialogService.DisplayAlertAsync("Alert", Constants.INTERNAL_ERROR, "Ok");
            }
        }
    }
}
