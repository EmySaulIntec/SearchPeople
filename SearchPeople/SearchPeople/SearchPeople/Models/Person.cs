using SearchPeople.Models.baseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchPeople.Models
{
    public class Person : Entity
    {
        public string Name { get; set; }

        public int GroupId { get; set; }

    }
}
