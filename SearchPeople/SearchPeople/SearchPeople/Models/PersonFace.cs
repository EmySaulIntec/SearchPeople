using SearchPeople.Models.baseEntity;
using System.IO;
using Xamarin.Forms;

namespace SearchPeople.Models
{
    public class PersonFace : Entity
    {
        public ImageSource Image { get; set; }
        public string Path { get; set; }

        public Stream ImageStream
        {
            get
            {
                return File.OpenRead(this.Path);
            }
        }
    }
}
