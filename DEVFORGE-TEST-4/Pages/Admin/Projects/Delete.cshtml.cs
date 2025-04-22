using Microsoft.EntityFrameworkCore;
using DEVFORGE_TEST_4.Services;
using DEVFORGE_TEST_4.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DEVFORGE_TEST_4.Pages.Admin.Projects
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            var project = _context.Projects
                .Include(p => p.ProjectTags)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                Response.Redirect("/Admin/Projects/Index");
                return;
            }

            
            if (!string.IsNullOrEmpty(project.ImageFileName))
            {
                var imagePath = Path.Combine("wwwroot", "img", "projects", project.ImageFileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            
            _context.ProjectTags.RemoveRange(project.ProjectTags);

            
            _context.Projects.Remove(project);

            _context.SaveChanges();

            Response.Redirect("/Admin/Projects/Index");
        }
    }
}
