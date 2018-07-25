using System.Collections.Generic;
using System.Linq;
using ArnimalService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArnimalService.Controllers
{
    [Route("api/animal")]
    [ApiController]
    public class AnimalController : Controller
    {
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

        [HttpPost("detect-animal")]
        public IActionResult Create([FromBody] string filePath)
        {

            // Upload to ML service
            // Check ML for animal
            // Get animal from ML
            // Resolve Animal ID
            // Send Animal Info
            //_context.Animals.Add(animal);
            //_context.SaveChanges();

            //return CreatedAtRoute("GetAnimal", new { id = animal.Id }, animal);
            return null;
        }

        [HttpPost("add-animal")]
        public IActionResult Create([FromBody]Animal animal)
        {
            // Get animal image folder
            // Insert it into ML and run ML

            _context.Animals.Add(animal);
            _context.SaveChanges();

            return CreatedAtRoute("GetAnimal", new { id = animal.Id }, animal);
        }

        // DELETE /api/animal/{id} 
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.Animals.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
