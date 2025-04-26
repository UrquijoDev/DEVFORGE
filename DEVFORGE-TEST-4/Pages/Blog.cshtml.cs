using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages
{
    public class BlogModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BlogModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserProfile> UserProfiles { get; set; } = new();
        public UserProfile? SelectedUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedUserId { get; set; }

        public void OnGet()
        {
            UserProfiles = _context.UserProfiles
                .Include(up => up.User)
                .ToList();

            if (SelectedUserId.HasValue)
            {
                SelectedUser = _context.UserProfiles
                    .Include(up => up.User)
                    .Include(up => up.Projects)
                        .ThenInclude(up => up.Project)
                            .ThenInclude(p => p.ProjectTags)
                                .ThenInclude(pt => pt.Tag)
                    .FirstOrDefault(up => up.Id == SelectedUserId.Value);
            }

            if (SelectedUser == null)
            {
                SelectedUser = _context.UserProfiles
                    .Include(up => up.User)
                    .Include(up => up.Projects)
                        .ThenInclude(up => up.Project)
                            .ThenInclude(p => p.ProjectTags)
                                .ThenInclude(pt => pt.Tag)
                    .FirstOrDefault();
            }
        }
    }
}
