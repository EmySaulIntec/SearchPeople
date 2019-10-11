using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchPeople.Services
{
    public class RecognitionAppService : IRecognitionAppService
    {
        const int MAX_TRANSACTION_COUNT = 5;


        private int PersonCreateds = 0;

        List<Person> People = new List<Person>();

        private IFaceServiceClient faceClient;
        public RecognitionAppService()
        {
            faceClient = new FaceServiceClient(Config.APIKey, Config.ApiUrl);
        }

        public async Task<PersonGroup[]> ListPersonGroupsAsync()
        {
            return await faceClient.ListPersonGroupsAsync();
        }

        public async Task<bool> CreateGroupAsync()
        {
            try
            {
                var groups = await faceClient.ListPersonGroupsAsync();

                if (groups.Any(e => e.PersonGroupId == Constants.PERSON_GROUP_ID))
                    await faceClient.DeletePersonGroupAsync(Constants.PERSON_GROUP_ID);

                await faceClient.CreatePersonGroupAsync(Constants.PERSON_GROUP_ID, Constants.PERSON_GROUP_ID);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Guid> CreatePerson(IEnumerable<Stream> trainingPathPerson, string namePerson)
        {
            CreatePersonResult person = await faceClient.CreatePersonAsync(Constants.PERSON_GROUP_ID, namePerson);

            var facesNotDetected = await AddFaceToPerson(Constants.PERSON_GROUP_ID, person, trainingPathPerson);

            await faceClient.TrainPersonGroupAsync(Constants.PERSON_GROUP_ID);

            await WaitForTrainedPersonGroup(Constants.PERSON_GROUP_ID);

            PersonCreateds += 1;

            People.Add(new Person()
            {
                Name = namePerson,
                PersonId = person.PersonId
            });

            return person.PersonId;
        }

        public async Task SearchPersonInPictures(IEnumerable<FileStream> pathSearchPeople, Action<string, IEnumerable<Person>> processImageAction = null, bool personTogueter = false)
        {
            int transactionCount = 0;

            if (faceClient == null)
                return;

            var trainingStatus = await faceClient.GetPersonGroupTrainingStatusAsync(Constants.PERSON_GROUP_ID);
            if (trainingStatus.Status != Status.Succeeded)
                return;

            foreach (FileStream pictureToSearch in pathSearchPeople)
            {
                if (transactionCount == MAX_TRANSACTION_COUNT)
                {
                    await Task.Delay(1000);
                    transactionCount = 0;
                }
                else
                    transactionCount += 1;

                var peopleIdentifiedInPictured = await IdentifyPersons(Constants.PERSON_GROUP_ID, pictureToSearch, personTogueter);

                if (peopleIdentifiedInPictured != null)
                    processImageAction?.Invoke(pictureToSearch.Name, peopleIdentifiedInPictured);
            }

            if (transactionCount == 10)
                await Task.Delay(1000);
        }

        public async Task DeleteGroup()
        {
            await faceClient.DeletePersonGroupAsync(Constants.PERSON_GROUP_ID);
        }

        public async Task DeletePerson(Guid personId)
        {
            await faceClient.DeletePersonAsync(Constants.PERSON_GROUP_ID, personId);

            await faceClient.TrainPersonGroupAsync(Constants.PERSON_GROUP_ID);

            await WaitForTrainedPersonGroup(Constants.PERSON_GROUP_ID);
        }



        private async Task<IEnumerable<Person>> IdentifyPersons(string personGroupId, FileStream streamPerson, bool personTogueter)
        {
            Face[] faces = await faceClient.DetectAsync(streamPerson);

            Guid[] faceIds = faces.Select(face => face.FaceId).ToArray();

            if (faceIds.Length == 0)
                return null;

            IdentifyResult[] results = await faceClient.IdentifyAsync(personGroupId, faceIds);

            IEnumerable<Person> peopleFindendInImage = People.Where(e => results.SelectMany(d => d.Candidates).Any(c => c.PersonId == e.PersonId));

            if ((personTogueter && results.Count(e => e.Candidates.Any()) == PersonCreateds) || (!personTogueter && results.Any(e => e.Candidates.Any())))
            {
                return People.Where(e => results.SelectMany(d => d.Candidates).Any(c => c.PersonId == e.PersonId));
            }

            return null;
        }
        private async Task WaitForTrainedPersonGroup(string personGroupId)
        {
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceClient.GetPersonGroupTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != Status.Running)
                    break;

                await Task.Delay(1000);
            }
        }

        private async Task<List<Stream>> AddFaceToPerson(string personGroupId, CreatePersonResult person, IEnumerable<Stream> personPictures)
        {
            List<Stream> facesNotDetected = new List<Stream>();

            foreach (var streamFace in personPictures)
            {
                try
                {
                    await faceClient.AddPersonFaceAsync(personGroupId, person.PersonId, streamFace);
                }
                catch (FaceAPIException ex)
                {
                    facesNotDetected.Add(streamFace);
                }
            }
            return facesNotDetected;
        }
    }
}
