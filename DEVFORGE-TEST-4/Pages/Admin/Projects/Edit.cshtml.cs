using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public ProjectDTO ProjectDTO { get; set; } = new ProjectDTO();

        [BindProperty]
        public string TagsAsString { get; set; } = string.Empty;

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            var project = context.Projects
                .Include(p => p.ProjectTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            ProjectDTO.Title = project.Title;
            ProjectDTO.Description = project.Description;
            ProjectDTO.Tags = project.ProjectTags.Select(pt => pt.Tag.Name).ToList();
            TagsAsString = string.Join(", ", ProjectDTO.Tags);
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor llene todos los campos requeridos.";
                return Page();
            }

            var project = context.Projects
                .Include(p => p.ProjectTags)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                errorMessage = "No se encontró el proyecto a editar.";
                return Page();
            }

            // Actualizar datos del proyecto
            project.Title = ProjectDTO.Title;
            project.Description = ProjectDTO.Description;

            // Eliminar tags actuales
            context.ProjectTags.RemoveRange(project.ProjectTags);

            // Procesar nuevas etiquetas
            var tagNames = TagsAsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            foreach (var tagName in tagNames)
            {
                var existingTag = context.Tags.FirstOrDefault(t => t.Name.ToLower() == tagName);

                if (existingTag == null)
                {
                    existingTag = new Tag { Name = tagName };
                    context.Tags.Add(existingTag);
                    context.SaveChanges(); // Guardar para tener el Id
                }

                project.ProjectTags.Add(new ProjectTag
                {
                    ProjectId = project.Id,
                    TagId = existingTag.Id
                });
            }

            context.SaveChanges();
            successMessage = "El proyecto fue actualizado correctamente.";
            return RedirectToPage("/Admin/Projects/Index");
        }
    }
}
