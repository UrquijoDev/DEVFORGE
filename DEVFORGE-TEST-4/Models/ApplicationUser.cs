using Microsoft.AspNetCore.Identity;

namespace DEVFORGE_TEST_4.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ImageFileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }    
            
    }
}
