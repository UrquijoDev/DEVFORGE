namespace DEVFORGE_TEST_4.Models
{
    public class UserProject
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
