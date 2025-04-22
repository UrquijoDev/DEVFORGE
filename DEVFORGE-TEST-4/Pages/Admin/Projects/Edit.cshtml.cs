using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectDTO ProjectData { get; set; } = new ProjectDTO();

        [BindProperty]
        public string TagsAsString { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string ExistingImage { get; set; } = string.Empty;
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            var project = _context.Projects
                .Include(p => p.ProjectTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            ProjectData.Title = project.Title;
            ProjectData.Description = project.Description;
            ProjectData.Tags = project.ProjectTags.Select(pt => pt.Tag.Name).ToList();
            TagsAsString = string.Join(", ", ProjectData.Tags);
            ExistingImage = project.ImageFileName;
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor llene todos los campos requeridos.";
                return Page();
            }

            var project = _context.Projects
                .Include(p => p.ProjectTags)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                errorMessage = "No se encontró el proyecto a editar.";
                return Page();
            }

            project.Title = ProjectData.Title;
            project.Description = ProjectData.Description;

            
            _context.ProjectTags.RemoveRange(project.ProjectTags);

            
            var tagNames = TagsAsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower())
                .Distinct()
                .ToList();

            foreach (var tagName in tagNames)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Name.ToLower() == tagName);
                if (existingTag == null)
                {
                    existingTag = new Tag { Name = tagName };
                    _context.Tags.Add(existingTag);
                    _context.SaveChanges();
                }

                project.ProjectTags.Add(new ProjectTag
                {
                    ProjectId = project.Id,
                    TagId = existingTag.Id
                });
            }

            
            if (ImageFile != null && ImageFile.Length > 0)
            {
                
                if (!string.IsNullOrEmpty(project.ImageFileName))
                {
                    string oldPath = Path.Combine("wwwroot", "img", "projects", project.ImageFileName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string imagePath = Path.Combine("wwwroot", "img", "projects");

                Directory.CreateDirectory(imagePath);
                string fullPath = Path.Combine(imagePath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                project.ImageFileName = fileName;
            }

            _context.SaveChanges();
            successMessage = "El proyecto fue actualizado correctamente.";
            return RedirectToPage("/Admin/Projects/Index");
        }
    }
}
