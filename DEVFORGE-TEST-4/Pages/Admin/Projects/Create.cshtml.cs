using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectDTO ProjectData { get; set; } = new ProjectDTO();

        [BindProperty]
        public string TagsAsString { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor completa todos los campos.";
                return Page();
            }

            var project = new Project
            {
                Title = ProjectData.Title,
                Description = ProjectData.Description,
                ProjectTags = new List<ProjectTag>()
            };

            
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string path = Path.Combine("wwwroot", "img", "projects");

                Directory.CreateDirectory(path);
                string fullPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                project.ImageFileName = fileName;
            }

            
            if (!string.IsNullOrWhiteSpace(TagsAsString))
            {
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
                        TagId = existingTag.Id,
                        Tag = existingTag
                    });
                }
            }

            _context.Projects.Add(project);
            _context.SaveChanges();

            successMessage = "Proyecto creado correctamente.";
            return RedirectToPage("/Admin/Projects/Index");
        }
    }
}
