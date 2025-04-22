using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<Project> Projects { get; set; } = new();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            Projects = context.Projects
                .Include(p => p.ProjectTags)
                .ThenInclude(pt => pt.Tag)
                .ToList();
        }
    }
}
