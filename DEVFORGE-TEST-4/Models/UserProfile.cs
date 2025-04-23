using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEVFORGE_TEST_4.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; }

        public string Bio { get; set; } = string.Empty;

        public string RoleVisible { get; set; } = string.Empty;

        public string GitHubUrl { get; set; } = string.Empty;

        public string LinkedInUrl { get; set; } = string.Empty;

        public string TwitterUrl { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty; // CSV: "C#, JS, SQL"

        public List<UserProject> Projects { get; set; } = new();
    }
}
