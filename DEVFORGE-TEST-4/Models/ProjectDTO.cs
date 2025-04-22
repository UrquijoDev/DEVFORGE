using System.ComponentModel.DataAnnotations;

namespace DEVFORGE_TEST_4.Models
{
    public class ProjectDTO
    {
        [Required(ErrorMessage = "El título del proyecto es obligatorio")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del proyecto es obligatoria")]
        public string Description { get; set; } = string.Empty;

        // Lista de nombres de etiquetas (pueden venir del input del usuario)
        public List<string> Tags { get; set; } = new();
    }
}
