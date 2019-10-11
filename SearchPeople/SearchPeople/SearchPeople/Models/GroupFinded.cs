using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SearchPeople.Models
{
    public class GroupFinded
    {
        public ImageSource MainImage
        {
            get
            {
                string path = PeoplePhotos.FirstOrDefault();
                if (!string.IsNullOrEmpty(path))
                    return ImageSource.FromFile(path);

                return null;
            }
        }
        public int CountImages
        {
            get
            {
                return SearchedPeople.Count;
            }
        }
        public string PeopleName { get; set; }

        public List<string> PeoplePhotos { get; set; } = new List<string>();
        public List<string> SearchedPeople { get; set; } = new List<string>();

    }
}
