using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using System.Collections.Generic;

namespace SearchPeople.Services.Dto
{
    public class Attributes
    {
        public FaceAttributes FaceAttributes { get; set; }
        public double Age { get; set; }
        public string Gender { get; set; }
        public double Smile { get; set; }
        public FacialHair FacialHair { get; set; }
        public HeadPose HeadPose { get; set; }
        public Glasses Glasses { get; set; }
        public EmotionScores Emotion { get; set; }

        public string GetInfo()
        {
            string haveGlasses = this.Glasses == Glasses.NoGlasses ? "" : this.Glasses.ToString();

            return $"A {Gender} with {this.Age} years old. {haveGlasses}";
        }
    }

    public class DetailImage
    {
        public DetailImage()
        {
            Attributes = new List<Attributes>();
        }
        public List<Attributes> Attributes { get; set; }
    }
}
