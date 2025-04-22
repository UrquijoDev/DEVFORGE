using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public CreateModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public ProjectDTO ProjectDTO { get; set; } = new ProjectDTO();

        [BindProperty]
        public string TagsAsString { get; set; } = string.Empty;

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet() { }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor, completa todos los campos requeridos.";
                return;
            }

            var project = new Project
            {
                Title = ProjectDTO.Title,
                Description = ProjectDTO.Description,
                ProjectTags = new List<ProjectTag>()
            };

            // Procesar etiquetas
            if (!string.IsNullOrWhiteSpace(TagsAsString))
            {
                var tagNames = TagsAsString
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim().ToLower())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .ToList();

                foreach (var tagName in tagNames)
                {
                    // Buscar si ya existe la etiqueta
                    var existingTag = context.Tags.FirstOrDefault(t => t.Name.ToLower() == tagName);

                    if (existingTag == null)
                    {
                        existingTag = new Tag { Name = tagName };
                        context.Tags.Add(existingTag);
                        context.SaveChanges(); // Se guarda para obtener el Id
                    }

                    // Asociar etiqueta al proyecto
                    project.ProjectTags.Add(new ProjectTag
                    {
                        TagId = existingTag.Id,
                        Tag = existingTag
                    });
                }
            }

            context.Projects.Add(project);
            context.SaveChanges();

            successMessage = "El proyecto fue creado exitosamente.";
            ModelState.Clear();
            ProjectDTO = new ProjectDTO();
            TagsAsString = "";

            Response.Redirect("/Admin/Projects/Index");
        }
    }
}
