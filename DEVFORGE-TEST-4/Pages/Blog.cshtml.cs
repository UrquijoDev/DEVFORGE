using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
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
        public UserProfile SelectedUser { get; set; }

        public void OnGet()
        {
            // Obtener todos los perfiles con la info básica
            UserProfiles = _context.UserProfiles
                .Include(up => up.User)
                .ToList();

            // Seleccionar el primero y traer sus proyectos y etiquetas
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
