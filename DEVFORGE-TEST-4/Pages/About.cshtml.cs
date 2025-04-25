using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages
{
    public class AboutModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AboutModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserProfile> TeamMembers { get; set; } = new();

        public void OnGet()
        {
            TeamMembers = _context.UserProfiles
                .Include(p => p.User)
                .ToList();
        }
    }
}
