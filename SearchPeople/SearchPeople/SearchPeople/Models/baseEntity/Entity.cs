using System;

namespace SearchPeople.Models.baseEntity
{
    public class Entity : IEntity
    {
        public int Id { get; set; }
        public DateTime Creationtime { get; set; }
    }
}
