using CRUDTest.Db;
using CRUDTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace CRUDTest.Controllers
{
    [Route("api/drillblockpoints")]
    [ApiController]
    public class DrillBlockPointsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public DrillBlockPointsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var points = await _appDbContext.DrillBlockPoints.ToListAsync();

            return Ok(points);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var drillblockPoint = await _appDbContext.DrillBlockPoints.FirstOrDefaultAsync(x => x.Id == id);
            if (drillblockPoint == null) return NotFound();

            return Ok(drillblockPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DrillBlockPointsInput input)
        {
            var drillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == input.DrillBlockId);
            if (drillblock == null) return NotFound();

            var exist = await _appDbContext.DrillBlockPoints.FirstOrDefaultAsync(x => x.X == input.X && x.Y == input.Y && x.Z == input.Z || x.Sequence == input.Sequence);
            if(exist != null)
            {
                return Problem(
                        title: "Unique constraint",
                        detail: "The point with the specified coordinates or sequence already exists"
                    );
            }

            var point = new DrillBlockPoint
            {
                X = input.X,
                Y = input.Y,
                Z = input.Z,
                Sequence = input.Sequence,
                DrillBlockId = drillblock.Id,
            };

            await _appDbContext.DrillBlockPoints.AddAsync(point);
            await _appDbContext.SaveChangesAsync();

            return Ok(point.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DrillBlockPointsInput input)
        {
            var drillblockPoint = await _appDbContext.DrillBlockPoints.FirstOrDefaultAsync(x => x.Id == id);
            if (drillblockPoint == null) return NotFound();

            if (drillblockPoint.DrillBlockId != input.DrillBlockId)
            {
                var prevDrillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == drillblockPoint.DrillBlockId);
                if (prevDrillblock == null) return NotFound();
                var drillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == input.DrillBlockId);
                if (drillblock == null) return NotFound();

                drillblockPoint.DrillBlockId = drillblock.Id;
            }

            var exist = await _appDbContext.DrillBlockPoints.FirstOrDefaultAsync(x => x.X == input.X && x.Y == input.Y && x.Z == input.Z || x.Sequence == input.Sequence);
            if (exist != null)
            {
                return Problem(
                        title: "Unique constraint",
                        detail: "The point with the specified coordinates or sequence already exists"
                    );
            }

            drillblockPoint.X = input.X;
            drillblockPoint.Y = input.Y;
            drillblockPoint.Z = input.Z;
            drillblockPoint.Sequence= input.Sequence;

            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var drillblockPoint = await _appDbContext.DrillBlockPoints.FirstOrDefaultAsync(x => x.Id == id);
            if (drillblockPoint == null) return NotFound();

            _appDbContext.DrillBlockPoints.Remove(drillblockPoint);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }
    }

    public class DrillBlockPointsInput
    {
        [Required]
        public int DrillBlockId { get; set; }
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
        [Required]
        public int Z { get; set; }
        [Required]
        public int Sequence { get; set; }
    }
}
