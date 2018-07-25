using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArnimalService.Models;
using AzureMLLibrary.Prediction;
using AzureMLLibrary.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create([FromForm] IFormCollection form)
        {
            var file = form.Files[0];
            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            using (var tempStream = System.IO.File.Create(tempPath))
            {
                form.Files[0].CopyTo(tempStream);
            }
            return Ok(CSPrediction.MakePredictionRequestByImagePath(tempPath).Result);
        }

        [HttpPost("add")]
        public IActionResult Create([FromBody]Animal animal)
        {
            // Get animal image folder
            var images = Directory.EnumerateFiles(animal.PathToTrainingImages).Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("png")).ToList();

            // Insert it into ML and run ML
            Guid imageTag = CSUploadAndTrain.UploadImageAndReturnTag(ProjectId, TrainingKey, images);
            bool trainSuccess = CSUploadAndTrain.Train(ProjectId, TrainingKey);

            AnimalMetadata metadata = new AnimalMetadata();
            metadata.Id = animal.Id;
            metadata.MLGuid = imageTag;
            metadata.Trained = trainSuccess;
            metadata.PathToTrainingImages = animal.PathToTrainingImages;

            _context.AnimalsMetadata.Add(metadata);
            _context.Animals.Add(animal);
            _context.SaveChanges();

            return CreatedAtRoute("GetAnimal", new { id = animal.Id }, animal);
        }

        // DELETE /api/animal/{id} 
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var animal = _context.Animals.Find(id);
            var metadata = _context.AnimalsMetadata.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            if (metadata != null)
            {
                CSUploadAndTrain.RemoveImagesByTag(ProjectId, TrainingKey, metadata.MLGuid);
            }

            _context.AnimalsMetadata.Remove(metadata);
            _context.Animals.Remove(animal);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
