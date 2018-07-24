using System.Collections.Generic;
using ArnimalService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArnimalService.Controllers
{
    [Route("api/[controller]")] 
    public class ValuesController : Controller
    {
        // GET <url>/animal
        // Returns all Animals
        [HttpGet]
        public IEnumerable<Animal> Get()
        {
            return new Animal[] {};
        }

        // POST <url>/animal
        // (Adds animal training images to training set and associate with other fields)
        [HttpPost("{animal}")]
        public bool Post([FromBody] Animal animal)
        {
            return true;
        }

        // POST <url>/detect
        // (Adds animal training images to training set and associate with other fields)
        [HttpPost("{detect}")]
        public Animal Post([FromBody]string filePath)
        {
            return null;
        }

        // DELETE <url>/animal/name
        //(deletes animal with name)
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
