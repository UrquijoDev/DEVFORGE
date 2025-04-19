using System.ComponentModel.DataAnnotations;

namespace DEVFORGE_TEST_4.Models
{
    public class ServicePlan
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Price { get; set; } = string.Empty;

        public List<Feature> Features { get; set; } = new();
    }


}