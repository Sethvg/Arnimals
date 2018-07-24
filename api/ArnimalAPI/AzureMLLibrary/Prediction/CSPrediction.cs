using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AzureMLLibrary.Prediction
{
    public class CSPrediction
    {
        const string PredictionUrl_ImagePath = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/76e820e5-5b11-49d5-872e-05a672e3689c/image";
        const string PredictionUrl_ImageUrl = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/76e820e5-5b11-49d5-872e-05a672e3689c/url";

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            if (string.IsNullOrEmpty(imageFilePath))
            {
                return new byte[] { };
            }

            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        public static async Task<string> MakePredictionRequestByImagePath(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", AuthenticationManager.GetPredictionKey());

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(CSPrediction.PredictionUrl_ImagePath, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> MakePredictionRequestByImageUrl(string imageUrl)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", AuthenticationManager.GetPredictionKey());

            HttpResponseMessage response;
            string contentJson = CSPrediction.CreateUrlContentBody(imageUrl);

            using (HttpContent content = new StringContent(contentJson))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(CSPrediction.PredictionUrl_ImageUrl, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static string CreateUrlContentBody(string imageUrl)
        {
            JObject jObject = new JObject();
            jObject.Add("Url", imageUrl);
            return jObject.ToString();
        }
    }
}
