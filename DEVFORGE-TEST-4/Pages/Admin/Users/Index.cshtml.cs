using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<UserProfile> UserProfiles { get; set; } = new();

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            UserProfiles = _context.UserProfiles
                .Include(up => up.User)
                .Include(up => up.Projects)
                    .ThenInclude(up => up.Project)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var profile = await _context.UserProfiles
                .Include(p => p.Projects)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound();
            }

            _context.UserProjects.RemoveRange(profile.Projects);
            _context.UserProfiles.Remove(profile);

            if (profile.User != null)
            {
                _context.Users.Remove(profile.User);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
