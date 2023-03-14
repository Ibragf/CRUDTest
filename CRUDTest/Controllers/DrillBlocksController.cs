using CRUDTest.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUDTest.Controllers
{
    [Route("api/drillblocks")]
    [ApiController]
    public class DrillBlocksController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public DrillBlocksController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drillBlocks = await _appDbContext.DrillBlocks.ToListAsync();

            return Ok(drillBlocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var drillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == id);
            if (drillblock == null) return NotFound();

            return Ok(drillblock);
        }

        [HttpGet("{id}/holes")]
        public async Task<IActionResult> GetHoles(int id)
        {
            var drillblock = await _appDbContext.DrillBlocks.Where(x => x.Id == id).Include(x => x.Holes).FirstOrDefaultAsync();
            if (drillblock == null) return NotFound();

            return Ok(drillblock.Holes);
        }

        [HttpGet("{id}/points")]
        public async Task<IActionResult> GetPoints(int id)
        {
            var drillblock = await _appDbContext.DrillBlocks.Where(x => x.Id == id).Include(x => x.DrillBlockPoints).FirstOrDefaultAsync();
            if (drillblock == null) return NotFound();

            return Ok(drillblock.DrillBlockPoints);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DrillBlockInput input)
        {
            var drillBlock = new DrillBlock
            {
                Name = input.Name,
                UpdateDate = input.UpdateDate,
            };

            await _appDbContext.DrillBlocks.AddAsync(drillBlock);
            await _appDbContext.SaveChangesAsync();

            return Ok(drillBlock.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DrillBlockInput input)
        {
            var drillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == id);
            if (drillblock == null) return NotFound();

            drillblock.UpdateDate = input.UpdateDate;
            drillblock.Name = input.Name;

            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var drillblock = await _appDbContext.DrillBlocks.FirstOrDefaultAsync(x => x.Id == id);
            if (drillblock == null) return NotFound();

            _appDbContext.DrillBlocks.Remove(drillblock);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }
    }

    public class DrillBlockInput
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }
    }
}
