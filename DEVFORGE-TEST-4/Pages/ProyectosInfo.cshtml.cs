using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages
{
    public class ProyectosInfoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProyectosInfoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Project Project { get; set; }

        public IActionResult OnGet(int id)
        {
            Project = _context.Projects
                .Include(p => p.ProjectTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefault(p => p.Id == id);

            if (Project == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
