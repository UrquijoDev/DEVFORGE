using System.ComponentModel.DataAnnotations;

namespace DEVFORGE_TEST_4.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título del proyecto es obligatorio")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del proyecto es obligatoria")]
        public string Description { get; set; } = string.Empty;

        // Relación muchos a muchos con Tags
        public List<ProjectTag> ProjectTags { get; set; } = new();
    }
}
