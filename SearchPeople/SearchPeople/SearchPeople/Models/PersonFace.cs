using SearchPeople.Models.baseEntity;

namespace SearchPeople.Models
{
    public class PersonFace : Entity
    {
        public int PersonId { get; set; }
        public string FaceImagePath { get; set; }
    }
}
