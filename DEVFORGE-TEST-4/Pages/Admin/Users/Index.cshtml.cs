using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
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
    }
}
