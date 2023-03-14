using CRUDTest.Db;
using CRUDTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUDTest.Controllers
{
    [Route("api/holes")]
    [ApiController]
    public class HolesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public HolesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var holes = await _appDbContext.Holes.ToListAsync();

            return Ok(holes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hole = await _appDbContext.Holes.FirstOrDefaultAsync(x => x.Id == id);
            if (hole == null) return NotFound();

            return Ok(hole);
        }

        [HttpGet("{id}/points")]
        public async Task<IActionResult> GetPoints(int id)
        {
            var hole = await _appDbContext.Holes.Where(x => x.Id == id).Include(x => x.HolePoints).FirstOrDefaultAsync();
            if (hole == null) return NotFound();

            return Ok(hole.HolePoints);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HolesInput input)
        {
            var drillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == input.DrillBlockId);
            if (drillblock == null) return NotFound();

            var hole = new Hole
            {
                Name = input.Name,
                DrillBlockId = input.DrillBlockId,
                DEPTH = input.DEPTH,
            };

            await _appDbContext.Holes.AddAsync(hole);
            await _appDbContext.SaveChangesAsync();

            return Ok(hole.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Add(int id)
        {
            var hole = await _appDbContext.Holes.FirstOrDefaultAsync(x => x.Id == id);
            if (hole == null) return NotFound();

            _appDbContext.Holes.Remove(hole);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }
    }

    public class HolesInput
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        public int DrillBlockId { get; set; }
        [Required]
        public int DEPTH { get; set; }
    }
}
