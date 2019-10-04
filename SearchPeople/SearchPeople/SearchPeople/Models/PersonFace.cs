using SearchPeople.Models.baseEntity;
using Xamarin.Forms;

namespace SearchPeople.Models
{
    public class PersonFace : Entity
    {
        public int PersonId { get; set; }
        public ImageSource Image { get; set; }
    }
}
