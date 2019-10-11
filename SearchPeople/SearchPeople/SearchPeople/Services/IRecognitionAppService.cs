using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    public interface IRecognitionAppService
    {
        Task<PersonGroup[]> ListPersonGroupsAsync();
        Task CreateGroupAsync();
        Task<Guid> CreatePerson(IEnumerable<Stream> trainingPathPerson, string namePerson);
        Task SearchPersonInPictures(IEnumerable<FileStream> pathSearchPeople, Action<string, IEnumerable<Person>> processImageAction = null, bool personTogueter = false);

        Task DeleteGroup();
        Task DeletePerson(Guid personId);
    }
}
