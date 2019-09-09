using SearchPeople.Models.baseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchPeople.Models
{
    public class PersonFace : Entity
    {
        public int PersonId { get; set; }
        public string FaceImagePath { get; set; }
    }
}
