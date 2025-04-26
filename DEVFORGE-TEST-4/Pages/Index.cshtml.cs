using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DEVFORGE_TEST_4.Models;
using Microsoft.EntityFrameworkCore;
using DEVFORGE_TEST_4.Services;

namespace DEVFORGE_TEST_4.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<UserProfile> TeamMembers { get; set; } = new();

        public async Task OnGetAsync()
        {
            TeamMembers = await _context.UserProfiles
                .Include(up => up.User)
                .OrderBy(up => up.User.FirstName)
                .ToListAsync();
        }
    }
}
