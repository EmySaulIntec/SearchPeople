using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SearchPeople.Models;
using SearchPeople.Services;
using SearchPeople.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SearchPeople.ViewModels
{
    public class GalleryPageViewModel : IInitialize, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PersonFace> ListedImages { get; set; } = new ObservableCollection<PersonFace>();
        public DelegateCommand<PersonFace> ViewImageCommand { get; set; }
        public DelegateCommand DetailImageCommand { get; private set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public string Name { get; set; }
        public float ValueSmile { get; set; } = 0;

        public float ValueSmilePercent
        {
            get
            {
                return ValueSmile * 100;
            }
        }
        public string DetailInfo { get; set; }

        public bool CanSave { get; set; }
        public bool ShowDelete { get; set; }

        public bool ShowingDetail { get; set; }

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

        private IPageDialogService _pageDialogService;
        private INavigationService _navigationService;
        private IMonkeyManager _monkeyManager;
        private INavigationParameters _parameters;
        public GalleryPageViewModel(IPageDialogService pageDialogService, IMonkeyManager monkeyManager, INavigationService navigationService)
        {
            _pageDialogService = pageDialogService;
            _monkeyManager = monkeyManager;
            _navigationService = navigationService;
            var recognitionAppService = new RecognitionAppService();

            ViewImageCommand = new DelegateCommand<PersonFace>((image) =>
            {
                SelectedImage = image;
                ShowingDetail = false;
            });

            DetailImageCommand = new DelegateCommand(async () =>
            {
                if (SelectedImage == null)
                {
                    await pageDialogService.DisplayAlertAsync("Alert", Constants.IMAGE_NOT_SELECTED, "Ok");
                    return;
                }
                var detail = await recognitionAppService.GetAttribtsFromImage(SelectedImage.ImageStream);

                DetailInfo = string.Join(", ", detail.Attributes.Select(e => e.GetInfo()));

                ValueSmile = detail.Attributes.Select(d => d.Emotion.Happiness).Average();
                ShowingDetail = true;
            });
        }

        public void Initialize(INavigationParameters parameters)
        {
            _parameters = parameters;

            if (_parameters.ContainsKey(Constants.GALLERY_IMAGES))
                ListedImages = (ObservableCollection<PersonFace>)_parameters[Constants.GALLERY_IMAGES];

            if (_parameters.ContainsKey(Constants.GALLERY_NAME))
                Name = (string)_parameters[Constants.GALLERY_NAME];

            if (_parameters.ContainsKey(Constants.GALLERY_DELETE_METHOD))
            {
                var person = (Person)_parameters[Constants.GALLERY_ITEM];
                var methodCommand = (DelegateCommand<FaceToDelete>)_parameters[Constants.GALLERY_DELETE_METHOD];

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

                ShowDelete = true;
            }

            if (_parameters.ContainsKey(Constants.GROUPED_FINDED))
            {
                CanSave = true;
                GroupFinded groupFinded = (GroupFinded)_parameters[Constants.GROUPED_FINDED];

                SaveCommand = new DelegateCommand(async () =>
                {

                    var listedGroups = _monkeyManager.GetMonkey<List<GroupFinded>>(Constants.GROUPED_FINDED);
                    if (listedGroups != null)
                    {
                        listedGroups.Item.Add(groupFinded);
                        _monkeyManager.SaveMokey<List<GroupFinded>>(listedGroups.Item, Constants.GROUPED_FINDED);
                    }
                    else
                    {
                        _monkeyManager.SaveMokey<List<GroupFinded>>(new List<GroupFinded>()
                        {
                            groupFinded
                        }, Constants.GROUPED_FINDED);
                    }

                    await _pageDialogService.DisplayAlertAsync("Alert", "Saved", "Ok");
                    await _navigationService.NavigateAsync(NavigationConstants.Home);

                });
            }
        }


    }
}
