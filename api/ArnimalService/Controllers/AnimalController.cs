using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ArnimalService.Models;
using System.Linq;

#region AnimalController
namespace ArnimalService.Controllers
{
    [Route("api/[controller]")]
    public class AnimalController : Controller
    {
        private readonly AnimalContext _context;
        #endregion

        public AnimalController(AnimalContext context)
        {
            _context = context;
        }

        #region snippet_GetAll
        [HttpGet]
        public IEnumerable<Animal> GetAll()
        {
            return _context.Animals.ToList();
        }

        #region snippet_GetByName
        [HttpGet("{string}", Name = "GetAnimal")]
        public IActionResult GetByName(string name)
        {
            var item = _context.Animals.FirstOrDefault(t => t.Name == name);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        #endregion
        #endregion

        #region snippet_Create
        [HttpPost("{animal}")]
        public IActionResult Create([FromBody] Animal item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Animals.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetAnimal", new { name = item.Name }, item);
        }
        #endregion

        #region snippet_Detect
        [HttpPost("{detect}")]
        public IActionResult Update([FromBody] string filePath)
        {
            // Interface with ML?
            //if (filePath == null)
            //{
            //    return BadRequest();
            //}

            //var animal = _context.Animals.FirstOrDefault(t => t.Name == name);
            //if (animal == null)
            //{
            //    return NotFound();
            //}

            //animal.Name = item.Name;

            //_context.Animals.Update(animal);
            //_context.SaveChanges();
            return new NoContentResult();
        }
        #endregion

        #region snippet_Delete
        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var animal = _context.Animals.FirstOrDefault(t => t.Name == name);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            _context.SaveChanges();
            return new NoContentResult();
        }
        #endregion
    }
}

