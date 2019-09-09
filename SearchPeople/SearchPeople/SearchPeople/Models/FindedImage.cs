using SearchPeople.Models.baseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchPeople.Models
{
    public class FindedImage : Entity
    {
        public int GroupId { get; set; }
        public string ImagePath { get; set; }
    }
}
