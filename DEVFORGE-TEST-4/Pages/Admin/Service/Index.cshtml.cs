using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DEVFORGE_TEST_4.Models;
namespace DEVFORGE_TEST_4.Pages.Admin.Service
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<ServicePlan> ServicePlan { get; set; } = new List<ServicePlan>();
        public List<Feature> Feature { get; set; } = new List<Feature>();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            // Obtener todos los planes de servicio
            ServicePlan = context.ServicePlan.ToList();

            // Obtener todas las características, o si quieres las características relacionadas con un plan, puedes hacer:
            Feature = context.Features.ToList();

        }
    }
}
