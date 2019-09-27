using SearchPeople.Models.baseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchPeople.Models
{
    public class Group : Entity
    {
        public string personGroupId { get; set; }
        public string name { get; set; }
        public object userData { get; set; }

        public string FolderPath { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
