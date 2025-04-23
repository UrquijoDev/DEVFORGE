using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages.Admin.Users
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserProfile UserProfile { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        [BindProperty(SupportsGet = false)]
        public List<int> SelectedProjectIds { get; set; } = new(); // <- importante el atributo correcto

        public List<Project> AllProjects { get; set; } = new();
        public string? CurrentImage { get; set; }
        public string successMessage = string.Empty;
        public string errorMessage = string.Empty;

        public void OnGet(int? id)
        {
            if (id == null)
            {
                errorMessage = "ID de perfil no proporcionado.";
                return;
            }

            UserProfile = _context.UserProfiles
                .Include(p => p.User)
                .Include(p => p.Projects)
                .ThenInclude(up => up.Project)
                .FirstOrDefault(p => p.Id == id);

            if (UserProfile == null)
            {
                errorMessage = "Perfil no encontrado.";
                return;
            }

            AllProjects = _context.Projects.OrderBy(p => p.Title).ToList();
            SelectedProjectIds = UserProfile.Projects.Select(up => up.ProjectId).ToList();
            CurrentImage = UserProfile.User?.ImageFileName;
        }

        public IActionResult OnPost()
        {
            AllProjects = _context.Projects.OrderBy(p => p.Title).ToList();

            if (!ModelState.IsValid)
            {
                errorMessage = "Formulario inválido.";
                return Page();
            }

            var profileInDb = _context.UserProfiles
                .Include(p => p.User)
                .Include(p => p.Projects)
                .FirstOrDefault(p => p.Id == UserProfile.Id);

            if (profileInDb == null)
            {
                errorMessage = "Perfil no encontrado.";
                return Page();
            }

            profileInDb.Bio = UserProfile.Bio;
            profileInDb.RoleVisible = UserProfile.RoleVisible;
            profileInDb.Skills = UserProfile.Skills;
            profileInDb.GitHubUrl = UserProfile.GitHubUrl;
            profileInDb.LinkedInUrl = UserProfile.LinkedInUrl;
            profileInDb.TwitterUrl = UserProfile.TwitterUrl;

            // Validar si User está presente antes de asignar imagen
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                var folderPath = Path.Combine("wwwroot", "img", "Users");
                Directory.CreateDirectory(folderPath);
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                if (profileInDb.User != null)
                {
                    profileInDb.User.ImageFileName = fileName;
                }
            }

            // Actualizar proyectos
            _context.UserProjects.RemoveRange(profileInDb.Projects);

            foreach (var projectId in SelectedProjectIds)
            {
                profileInDb.Projects.Add(new UserProject
                {
                    ProjectId = projectId,
                    UserProfileId = profileInDb.Id
                });
            }

            _context.SaveChanges();
            successMessage = "Perfil actualizado correctamente.";
            return RedirectToPage("/Admin/Users/Index");
        }
    }
}
