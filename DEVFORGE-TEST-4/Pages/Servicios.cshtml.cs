
using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace DEVFORGE_TEST_4.Pages
{
    public class ServiciosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ServiciosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ServicePlan> ServicePlans { get; set; } = new();

        public async Task OnGetAsync()
        {
            ServicePlans = await _context.ServicePlan
                .Include(p => p.Features)
                .ToListAsync();
        }
    }
}
