using System.ComponentModel.DataAnnotations;

namespace DEVFORGE_TEST_4.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la etiqueta es obligatorio")]
        public string Name { get; set; } = string.Empty;

        public List<ProjectTag> ProjectTags { get; set; } = new();
    }
}
