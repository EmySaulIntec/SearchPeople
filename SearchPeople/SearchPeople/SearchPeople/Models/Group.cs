using Newtonsoft.Json;
using SearchPeople.Models.baseEntity;

namespace SearchPeople.Models
{
    public class Group : Entity
    {
        [JsonProperty("personGroupId")]
        public string PersonGroupId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonIgnore]
        public string FolderPath { get; set; }

        public override string ToString()
        {
            return $"{Name} ({PersonGroupId})";
        }
    }
}
