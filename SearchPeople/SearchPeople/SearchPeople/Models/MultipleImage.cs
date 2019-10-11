using Xamarin.Forms;

namespace SearchPeople.Models
{
    public class MultipleImage
    {
        public bool IsImageA
        {
            get
            {
                return ImageA != null;
            }
        }
        public bool IsImageB
        {
            get
            {
                return ImageB != null;
            }
        }

        public bool IsImageC
        {
            get
            {
                return ImageC != null;
            }
        }

        public ImageSource ImageA { get; set; }
        public ImageSource ImageB { get; set; }
        public ImageSource ImageC { get; set; }
    }
}
