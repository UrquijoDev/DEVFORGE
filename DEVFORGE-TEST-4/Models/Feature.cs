using System.ComponentModel.DataAnnotations;
namespace DEVFORGE_TEST_4.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int ServicePlanId { get; set; }

        public ServicePlan ServicePlan { get; set; }
    }
}
