using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject is null)
                return NotFound();

            celestialObject.Satellites.Add(celestialObject);

            return Ok(celestialObject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObjectList = _context.CelestialObjects.Where(x => x.Name == name);

            if (!celestialObjectList.Any())
                return NotFound();

            foreach (var item in celestialObjectList)
            {
                item.Satellites.Add(item);
            }

            return Ok(celestialObjectList);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjectList = _context.CelestialObjects;

            foreach (var item in celestialObjectList)
            {
                item.Satellites.Add(item);
            }

            return Ok(celestialObjectList);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CelestialObject celestialObject)
        {
            var celestial = _context.CelestialObjects.Find(id);

            if (celestial is null)
                return NotFound();

            celestial.Name = celestialObject.Name;
            celestial.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celestial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject([FromRoute] int id, [FromRoute] string name)
        {
            var celestial = _context.CelestialObjects.Find(id);

            if (celestial is null)
                return NotFound();

            celestial.Name = name;

            _context.CelestialObjects.Update(celestial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var celestial = _context.CelestialObjects
                .Where(x => x.Id == id || x.OrbitedObjectId == id);

            if (!celestial.Any())
                return NotFound();

            _context.CelestialObjects.RemoveRange(celestial);

            _context.SaveChanges();

            return NoContent();
        }
    }
}
