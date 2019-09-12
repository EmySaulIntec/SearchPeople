using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SearchPeople.Utils
{
    public class MediaHelper
    {
        public class ImagePhoto
        {
            public ImageSource Image { get; set; }
            public string Path { get; set; }
            public FileStream StreamImage { get; set; }
            public string Text { get; set; }
        }

        public static ImageSource GetImageFromPath(string path)
        {
            return ImageSource.FromFile(path);
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
                    PhotoSize = PhotoSize.Medium
                });
            else
                file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });

            if (file == null)
                return null;

            return new ImagePhoto()
            {
                Image = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                }),
                Path = file.Path,
                StreamImage = file.GetStream() as FileStream
            };

        }

        public async Task<IEnumerable<ImagePhoto>> PickMultiplePhotosAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return null;
            }

            List<MediaFile> files = await CrossMedia.Current.PickPhotosAsync(new Plugin.Media.Abstractions.PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            });

            if (files.Count == 0)
                return null;

            return files.Select(file => new ImagePhoto()
            {
                Image = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                }),
                Path = file.Path,
                StreamImage = file.GetStream() as FileStream
            });

        }
    }
}
