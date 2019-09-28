using SearchPeople.Models.baseEntity;

namespace SearchPeople.Models
{
    public class Person : Entity
    {
        public string Name { get; set; }

        public int GroupId { get; set; }

    }
}
