using SearchPeople.Models.baseEntity;

namespace SearchPeople.Models
{
    public class FindedImage : Entity
    {
        public int GroupId { get; set; }
        public string ImagePath { get; set; }
    }
}
