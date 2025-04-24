using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DEVFORGE_TEST_4.Models;
using DEVFORGE_TEST_4.Services;

namespace DEVFORGE_TEST_4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetProfileData(int id)
        {
            var profile = _context.UserProfiles
                .Include(p => p.User)
                .Include(p => p.Projects)
                    .ThenInclude(up => up.Project)
                        .ThenInclude(p => p.ProjectTags)
                            .ThenInclude(pt => pt.Tag)
                .FirstOrDefault(p => p.Id == id);

            if (profile == null) return NotFound();

            return new JsonResult(new
            {
                profile.RoleVisible,
                profile.Bio,
                profile.Skills,
                Email = profile.User?.Email,
                Phone = profile.User?.PhoneNumber,
                GitHubUrl = profile.GitHubUrl,
                TwitterUrl = profile.TwitterUrl,
                LinkedInUrl = profile.LinkedInUrl,
                Projects = profile.Projects.Select(up => new
                {
                    up.Project.Id,
                    up.Project.Title,
                    up.Project.Description,
                    up.Project.ImageFileName,
                    Tags = up.Project.ProjectTags.Select(pt => pt.Tag.Name)
                })
            });
        }
    }
}
