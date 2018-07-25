using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArnimalService.Models;
using AzureMLLibrary.Prediction;
using AzureMLLibrary.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArnimalService.Controllers
{
    [Route("api/animal")]
    [ApiController]
    public class AnimalController : Controller
    {
        private static string TrainingKey = "4a1c89faa74d479cb97ee40e93bc140d";
        private static Guid ProjectId = Guid.Parse("76e820e5-5b11-49d5-872e-05a672e3689c");

        private readonly AnimalContext _context;

        public AnimalController(AnimalContext context)
        {
            _context = context;
        }

        // GET /api/animal
        [HttpGet]
        public ActionResult<List<Animal>> GetAll()
        {
            return _context.Animals.ToList();
        }

        // GET /api/animal/{id} 
        [HttpGet("{id}", Name = "GetAnimal")]
        public ActionResult<Animal> GetById(long id)
        {
            var item = _context.Animals.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost("detect")]
        public IActionResult Detect([FromForm] IFormCollection form)
        {
            var file = form.Files[0];
            string response = CSPrediction.MakePredictionRequestByImagePath(saveFormFile(file)).Result;
            JObject predictionObj = (JObject)JsonConvert.DeserializeObject(response);
            JToken token = predictionObj.GetValue("predictions");

            SortedDictionary<double, Animal> animals = new SortedDictionary<double, Animal>();
            foreach (JToken child in token.Children())
            {
                double prob = child.SelectToken("probability").ToObject<double>();
                string tagId = child.SelectToken("tagId").ToObject<string>();

                Animal animal = this._context.Animals.Where(a => a.Id.ToLower().Equals(tagId.ToLower()))?.FirstOrDefault();
                if (animal != null)
                {
                    animals.Add(prob, animal);
                }
            }

            if (animals.Any())
            {
                return Ok(animals.TakeLast(2));
            }
            return NotFound();
        }

        private string saveFormFile(IFormFile file)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            using (var tempStream = System.IO.File.Create(tempPath))
            {
                file.CopyTo(tempStream);
            }
            return tempPath;
        }

        [HttpPost("add")]
        public IActionResult Add([FromForm] IFormCollection form)
        {

            List<string> fileList = new List<string>();
            string profilePath = "";
            foreach(IFormFile file in form.Files)
            {
                string saved = this.saveFormFile(file);
                fileList.Add(saved);

                if (string.IsNullOrWhiteSpace(profilePath))
                {
                    var fName = Guid.NewGuid().ToString() + ".jpg";
                   var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "static", "images",fName);
                    profilePath = "/images/" + fName;

                    using (var tempStream = System.IO.File.Create(tempPath))
                    {
                        file.CopyTo(tempStream);
                    }

                }

            }

            Animal animal = null;
            foreach (KeyValuePair<string, StringValues> key in form)
            {
                if (key.Key.Equals("animal"))
                {
                    animal = JsonConvert.DeserializeObject<Animal>(key.Value[0]);
                }
            }

            // Insert it into ML and run ML
            Guid imageTag = CSUploadAndTrain.UploadImageAndReturnTag(ProjectId, TrainingKey, fileList);
            bool trainSuccess = CSUploadAndTrain.Train(ProjectId, TrainingKey);


            animal.Img = profilePath;
            animal.Id = imageTag.ToString();
            this._context.Animals.Add(animal);
            this._context.SaveChanges();
            return Ok(animal);
        }

        // DELETE /api/animal/{id} 
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var animal = _context.Animals.Find(id);

            CSUploadAndTrain.RemoveImagesByTag(ProjectId, TrainingKey, Guid.Parse(animal.Id));

            _context.Animals.Remove(animal);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
