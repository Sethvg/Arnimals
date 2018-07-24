using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Cognitive.CustomVision.Training;

namespace AzureMLLibrary.Training
{
     public static class CSUploadAndTrain
    {
        public static bool RemoveImagesByTag(Guid projectId, string trainingKey, Guid tagId)
        {
            TrainingApi trainingApi = new TrainingApi() { ApiKey = trainingKey };
            try
            {
                trainingApi.DeleteTag(projectId, tagId);
                Console.WriteLine("delete tagid " + tagId.ToString() + "!");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.ToString());
                return false;
            }
        }

        public static Guid UploadImageAndReturnTag(Guid projectId, string trainingKey, List<string> images)
        {
            // Create the Api, passing in the training key
            TrainingApi trainingApi = new TrainingApi() { ApiKey = trainingKey };
            // Make new tag in the project
            Guid newTag = Guid.NewGuid();
            var newAnimalTag = trainingApi.CreateTag(projectId, newTag.ToString());

            // Images can be uploaded one at a time
            try
            {
                foreach (var image in images)
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                    {
                        trainingApi.CreateImagesFromData(projectId, stream, new List<string>() { newAnimalTag.Id.ToString() });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.ToString());
                return Guid.Empty;
            }

            return newAnimalTag.Id;
        }

        public static bool Train(Guid projectId, string trainingKey)
        {
            // Create the Api, passing in the training key
            TrainingApi trainingApi = new TrainingApi() { ApiKey = trainingKey };
            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");
            try
            {
                var iteration = trainingApi.TrainProject(projectId);

                // The returned iteration will be in progress, and can be queried periodically to see when it has completed
                while (iteration.Status == "Training")
                {
                    Thread.Sleep(1000);

                    // Re-query the iteration to get it's updated status
                    iteration = trainingApi.GetIteration(projectId, iteration.Id);
                    Console.WriteLine(iteration.Status);
                }

                // The iteration is now trained. Make it the default project endpoint
                iteration.IsDefault = true;
                trainingApi.UpdateIteration(projectId, iteration.Id, iteration);
                Console.WriteLine("Done!\n");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return false;
            }
        }
    }
}
