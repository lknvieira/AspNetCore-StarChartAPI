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
    }
}
