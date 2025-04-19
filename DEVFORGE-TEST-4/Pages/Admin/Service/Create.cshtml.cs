using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DEVFORGE_TEST_4.Pages.Admin.Service
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext context;
        [BindProperty]
        public ServiceDTO ServiceDTO { get; set; } = new ServiceDTO();
        [BindProperty]
        public string FeaturesAsString { get; set; } = string.Empty;

        public CreateModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Porfavor llene todos los campos requeridos";
                return;
            }

            // Creamos el nuevo ServicePlan
            var servicePlan = new ServicePlan
            {
                Name = ServiceDTO.Name,
                Price = ServiceDTO.Price,
            };

            context.ServicePlan.Add(servicePlan);
            context.SaveChanges(); // Necesario para obtener el ID del plan

            // Agregar features si hay texto ingresado
            if (!string.IsNullOrWhiteSpace(FeaturesAsString))
            {
                var features = FeaturesAsString
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim())
                    .Where(f => !string.IsNullOrWhiteSpace(f))
                    .ToList();

                foreach (var featureText in features)
                {
                    var feature = new Feature
                    {
                        Description = featureText,
                        ServicePlanId = servicePlan.Id
                    };

                    context.Features.Add(feature);
                }

                context.SaveChanges();
            }
            ModelState.Clear();
            successMessage = "El plan fue creado exitosamente.";
            ServiceDTO = new ServiceDTO(); // Limpiar campos
            FeaturesAsString = "";
            Response.Redirect("/Admin/Service/Index");
        }

    }
 }
