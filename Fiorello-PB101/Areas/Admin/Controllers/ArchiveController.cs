using Fiorello_PB101.Data;
using Fiorello_PB101.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello_PB101.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArchiveController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public ArchiveController(
            ICategoryService categoryService,
            AppDbContext context
        )
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> CategoryArchive()
        {
            return View(await _categoryService.GetAllArchiveAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUnarchive(int? id)
        {
            if (id is null) return BadRequest();

            var category = await _categoryService.GetByIdAsync((int)id);

            if (category is null) return NotFound();

            category.SoftDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}
