using Microsoft.EntityFrameworkCore;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DEVFORGE_TEST_4.Pages.Admin.Service
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
                Response.Redirect("/Admin/Service/Index");
                return;
            }

            var servicePlan = context.ServicePlan
                .Include(sp => sp.Features)
                .FirstOrDefault(sp => sp.Id == id);

            if (servicePlan == null)
            {
                Response.Redirect("/Admin/Service/Index");
                return;
            }

            // Eliminar features asociadas primero
            context.Features.RemoveRange(servicePlan.Features);

            // Luego eliminar el service plan
            context.ServicePlan.Remove(servicePlan);

            context.SaveChanges();

            Response.Redirect("/Admin/Service/Index");
        }

    }
}
