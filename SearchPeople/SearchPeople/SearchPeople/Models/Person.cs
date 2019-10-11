using SearchPeople.Models.baseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SearchPeople.Models
{
    public class Person : Entity
    {
        public string Name { get; set; }

        public int GroupId { get; set; }

        public ImageSource MainImage
        {
            get
            {

                return this.TrainingTempImages.FirstOrDefault()?.Image;
            }
        }

        public List<PersonFace> TrainingTempImages { get; set; } = new List<PersonFace>();

        public int CountImages
        {
            get
            {
                return TrainingTempImages.Count;
            }
        }

        public Guid PersonId { get; set; }
    }
}
