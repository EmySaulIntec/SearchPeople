using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SearchPeople.Utils
{
    public class MediaHelper : IMediaHelper
    {
        public class ImagePhoto
        {
            public ImageSource Image
            {
                get
                {
                    return ImageSource.FromFile(this.Path);
                }
            }
            public string Path { get; set; }
            public string Text { get; set; }
            public FileStream StreamImage { get; set; }
        }

        public ImageSource GetImageFromPath(string path)
        {
            return ImageSource.FromFile(path);
        }

        public async Task<IEnumerable<ImagePhoto>> PickMultipleImages()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Allowed", ":( Not access.", "OK");
                return null;
            }

            var files = await CrossMedia.Current.PickPhotosAsync(new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Full
            });

            if (!files.Any())
                return null;

            return files.Select(e =>
             {
                 return new ImagePhoto()
                 {
                     Path = e.Path,
                     StreamImage = new FileStream(e.Path, FileMode.Open)
                 };

             });

        }

        public async Task<ImagePhoto> TakePhotoAsync(bool camera = false)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return null;
            }

            MediaFile file;
            if (camera)
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Pictures",
                    Name = "test.jpg",
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Full
                });
            else
                file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Full
                });

            if (file == null)
                return null;

            return new ImagePhoto()
            {
                Path = file.Path
            };

        }
    }
}
