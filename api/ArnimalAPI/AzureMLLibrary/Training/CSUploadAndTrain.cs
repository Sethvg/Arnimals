using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// CS0649 Filed will never be assigned to. Very noisy in this file because fields are assigned via reflection.
#pragma warning disable CS0649

namespace AzureMLLibrary.Training
{
    public static class CSUploadAndTrain
    {
        public static bool RemoveImagesByTag(Guid projectId, string trainingKey, Guid tagId)
        {
            try
            {
                bool result = CSUploadAndTrain.DeleteTagAsync(trainingKey, projectId, tagId.ToString()).Result;
                Console.WriteLine($"delete tagid {tagId}! {result}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.ToString());
                return false;
            }
        }

        private static async Task<bool> DeleteTagAsync(string subscriptionKey, Guid projectId, string tagId)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Training-Key", "");
            client.DefaultRequestHeaders.Add("Training-key", subscriptionKey);

            var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/{projectId}/tags/{tagId}?" + queryString;

            var response = await client.DeleteAsync(uri);
            Console.WriteLine("Delete: " + response);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public static Guid UploadImageAndReturnTag(Guid projectId, string trainingKey, List<string> images)
        {
            // Create the Api, passing in the training key
            // Make new tag in the project
            Guid newTag = Guid.NewGuid();

            var newAnimalTag = CSUploadAndTrain.CreateTagAsync(trainingKey, projectId, newTag.ToString()).Result;

            // Images can be uploaded one at a time
            try
            {
                foreach (var image in images)
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                    {
                        var response = CSUploadAndTrain.CreateImagesFromDataAsync(trainingKey, projectId, stream, new List<string>() { newAnimalTag.Id.ToString() }).Result;
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Console.WriteLine(response);
                            return Guid.Empty;
                        }
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

        private struct CreateTagResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int ImageCount { get; set; }
        }

        private static async Task<CreateTagResponse> CreateTagAsync(string trainingKey, Guid projectId, string tagName, string description = null)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Training-Key", "");
            client.DefaultRequestHeaders.Add("Training-key", trainingKey);

            // Request parameters
            if (description != null)
            {
                queryString["description"] = description;
            }
            var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/{projectId}/tags?name={tagName}&" + queryString;

            // Request body
            HttpResponseMessage response;
            using (var content = new StringContent(string.Empty))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                response = await client.PostAsync(uri, content);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new CreateTagResponse();
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateTagResponse>(responseBody, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private static async Task<HttpResponseMessage> CreateImagesFromDataAsync(string subscriptionKey, Guid projectId, Stream imageStream, IEnumerable<string> tags)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Training-Key", "");
            client.DefaultRequestHeaders.Add("Training-key", subscriptionKey);

            // Request parameters
            queryString["tagIds"] = string.Join(",", tags);
            var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/{projectId}/images?" + queryString;

            // Request body
            using (var innerContent = new StreamContent(imageStream))
            using (var content = new MultipartFormDataContent())
            {
                content.Add(innerContent, "image", "image.jpg");
                return await client.PostAsync(uri, content);
            }
        }

        public static bool Train(Guid projectId, string trainingKey)
        {
            // Create the Api, passing in the training key
            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");
            try
            {
                var iteration = CSUploadAndTrain.TrainProject(trainingKey, projectId).Result;
                if (iteration.Id == null)
                {
                    return false;
                }

                // The returned iteration will be in progress, and can be queried periodically to see when it has completed
                while (iteration.Status == "Training")
                {
                    Thread.Sleep(1000);

                    // Re-query the iteration to get it's updated status
                    iteration = CSUploadAndTrain.GetIteration(trainingKey, projectId, iteration.Id).Result;
                    if (iteration.Status == null)
                    {
                        return false;
                    }

                    Console.WriteLine(iteration.Status);
                }

                // The iteration is now trained. Make it the default project endpoint
                iteration.IsDefault = true;
                var updateResponse = CSUploadAndTrain.UpdateIteration(trainingKey, projectId, iteration).Result;
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(updateResponse);
                    return false;
                }

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

        private static async Task<Iteration> TrainProject(string subscriptionKey, Guid projectId)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Training-Key", "");
            client.DefaultRequestHeaders.Add("Training-key", subscriptionKey);

            var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/{projectId}/train?" + queryString;

            HttpResponseMessage response;

            // Request body
            using (var content = new StringContent(string.Empty))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                response = await client.PostAsync(uri, content);
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"TrainProject StatusCode: {response.StatusCode} Body: {responseBody}");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new Iteration();
            }

            return JsonConvert.DeserializeObject<Iteration>(responseBody, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private static async Task<Iteration> GetIteration(string subscriptionKey, Guid projectId, string iterationId)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Training-Key", "");
            client.DefaultRequestHeaders.Add("Training-key", subscriptionKey);

            var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/{projectId}/iterations/{iterationId}?" + queryString;

            var response = await client.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Iteration>(responseBody, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private static async Task<HttpResponseMessage> UpdateIteration(string subscriptionKey, Guid projectId, Iteration iteration)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Training-Key", "");
            client.DefaultRequestHeaders.Add("Training-key", subscriptionKey);

            string body = JsonConvert.SerializeObject(iteration, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            using (StringContent content = new StringContent(body))
            {
                var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/{projectId}/iterations/{iteration.Id}?" + queryString;
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return await client.PatchAsync(uri, content);
            }
        }

        private struct Iteration
        {
            public string Id;
            public string Name;
            public bool IsDefault;
            public string Status;
            public string Created;
            public string LastModified;
            public string TrainedAt;
            public string ProjectId;
            public bool Exportable;
            public string DomainId;
        }
    }
}
