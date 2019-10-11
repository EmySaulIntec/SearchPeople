using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using static SearchPeople.Utils.MediaHelper;

namespace SearchPeople.Utils
{
    public interface IMediaHelper
    {
        ImageSource GetImageFromPath(string path);
        Task<IEnumerable<ImagePhoto>> PickMultipleImages();
        Task<ImagePhoto> TakePhotoAsync(bool camera = false);

    }
}