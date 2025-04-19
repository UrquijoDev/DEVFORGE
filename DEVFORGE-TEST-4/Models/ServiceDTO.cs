using System.ComponentModel.DataAnnotations;

namespace DEVFORGE_TEST_4.Models
{
    public class ServiceDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        public string Price { get; set; } = string.Empty;

        // Puedes validarlo si quieres que al menos haya un feature
        public List<string> Features { get; set; } = new List<string>();
    }
}
