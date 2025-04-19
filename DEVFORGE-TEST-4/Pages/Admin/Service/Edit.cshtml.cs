using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DEVFORGE_TEST_4.Pages.Admin.Service
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ServiceDTO ServiceDTO { get; set; } = new ServiceDTO();

        [BindProperty]
        public string FeaturesAsString { get; set; } = string.Empty;

        public int ServicePlanId { get; set; }

        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Service/Index");
                return;
            }

            var service = context.ServicePlan
                .Include(sp => sp.Features)
                .FirstOrDefault(sp => sp.Id == id);

            if (service == null)
            {
                Response.Redirect("/Admin/Service/Index");
                return;
            }

            ServicePlanId = service.Id;
            ServiceDTO.Name = service.Name;
            ServiceDTO.Price = service.Price;
            ServiceDTO.Features = service.Features.Select(f => f.Description).ToList();
            FeaturesAsString = string.Join(", ", ServiceDTO.Features);
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor llene todos los campos requeridos.";
                return Page();
            }

            var servicePlan = context.ServicePlan
                .Include(sp => sp.Features)
                .FirstOrDefault(sp => sp.Id == id);

            if (servicePlan == null)
            {
                errorMessage = "No se encontró el plan a editar.";
                return Page();
            }

            // Actualizar datos del plan
            servicePlan.Name = ServiceDTO.Name;
            servicePlan.Price = ServiceDTO.Price;

            // Actualizar features
            var newFeatures = FeaturesAsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.Trim())
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .ToList();

            // Eliminar las actuales
            context.Features.RemoveRange(servicePlan.Features);

            // Agregar las nuevas
            foreach (var featureDesc in newFeatures)
            {
                servicePlan.Features.Add(new Feature
                {
                    Description = featureDesc,
                    ServicePlanId = servicePlan.Id
                });
            }

            context.SaveChanges();
            successMessage = "El plan fue actualizado correctamente.";
            return RedirectToPage("/Admin/Service/Index");
        }
    }
}
