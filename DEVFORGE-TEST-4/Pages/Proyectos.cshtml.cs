using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages
{
    public class ProyectosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProyectosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Project> Projects { get; set; } = new();

        public void OnGet()
        {
            Projects = _context.Projects
                .Include(p => p.ProjectTags)
                    .ThenInclude(pt => pt.Tag)
                .ToList();
        }
    }
}
