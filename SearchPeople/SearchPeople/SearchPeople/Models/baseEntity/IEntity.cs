using System;
using System.Collections.Generic;
using System.Text;

namespace SearchPeople.Models.baseEntity
{
    public interface IEntity
    {
        int Id { get; set; }

        DateTime Creationtime { get; set; }
    }
}
