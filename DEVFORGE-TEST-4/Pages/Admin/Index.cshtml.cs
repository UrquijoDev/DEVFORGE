using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Project> ProjectPreview { get; set; } = new();
        public List<ServicePlan> ServicePreview { get; set; } = new();
        public List<UserProfile> UserPreview { get; set; } = new();

        public void OnGet()
        {
            ProjectPreview = _context.Projects
                .OrderByDescending(p => p.Id)
                .Take(7)
                .ToList();

            ServicePreview = _context.ServicePlan
                .OrderByDescending(s => s.Id)
                .Take(3)
                .ToList();

            UserPreview = _context.UserProfiles
                .Include(up => up.User)
                .OrderByDescending(u => u.Id)
                .Take(3)
                .ToList();
        }
    }
}
