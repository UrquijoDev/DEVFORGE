using Microsoft.EntityFrameworkCore;
using DEVFORGE_TEST_4.Services;
using DEVFORGE_TEST_4.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public DeleteModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            var project = context.Projects
                .Include(p => p.ProjectTags)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            // Eliminar relaciones con tags
            context.ProjectTags.RemoveRange(project.ProjectTags);

            // Eliminar el proyecto
            context.Projects.Remove(project);

            context.SaveChanges();

            Response.Redirect("/Admin/Projects/Index");
        }
    }
}
