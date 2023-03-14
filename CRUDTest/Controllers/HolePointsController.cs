using CRUDTest.Db;
using CRUDTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUDTest.Controllers
{
    [Route("api/holepoints")]
    [ApiController]
    public class HolePointsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public HolePointsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var holesPoints = await _appDbContext.HolePoints.ToListAsync();

            return Ok(holesPoints);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var holePoint = await _appDbContext.HolePoints.FirstOrDefaultAsync(x => x.Id == id);
            if (holePoint == null) return NotFound();

            return Ok(holePoint);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HolePointsInput input)
        {
            var hole = await _appDbContext.Holes.FirstOrDefaultAsync(x => x.Id == input.HoleId);
            if(hole == null) return NotFound();

            var exist = await _appDbContext.HolePoints.FirstOrDefaultAsync(x => x.X == input.X && x.Y == input.Y && x.Z == input.Z);
            if (exist != null)
            {
                return Problem(
                        title: "Unique constraint",
                        detail: "The point with the specified coordinates or sequence already exists"
                    );
            }

            var point = new HolePoint
            {
                X = input.X,
                Y = input.Y,
                Z = input.Z,
                HoleId = input.HoleId,
            };

            await _appDbContext.HolePoints.AddAsync(point);
            await _appDbContext.SaveChangesAsync();

            return Ok(point.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var point = await _appDbContext.HolePoints.FirstOrDefaultAsync(x => x.Id == id);
            if (point == null) return NotFound();

            _appDbContext.HolePoints.Remove(point);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }
    }

    public class HolePointsInput
    {
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
        [Required]
        public int Z { get; set; }
        [Required]
        public int HoleId { get; set; }
    }
}
