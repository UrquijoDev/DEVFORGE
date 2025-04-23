using DEVFORGE_TEST_4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DEVFORGE_TEST_4.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ServicePlan> ServicePlan { get; set; }
        public DbSet<Feature> Features { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProjectTag> ProjectTags { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole
            {
                Id = "cef35870-6069-4aa3-8c7c-1ba13c37ca60",
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            var cliente = new IdentityRole
            {
                Id = "33b9e041-82a4-48b5-8b50-2e79162389dd",
                Name = "cliente",
                NormalizedName = "CLIENTE"
            };

            builder.Entity<IdentityRole>().HasData(admin, cliente);

            builder.Entity<ServicePlan>()
              .HasMany(p => p.Features)
              .WithOne(f => f.ServicePlan)
              .HasForeignKey(f => f.ServicePlanId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectTag>()
                .HasKey(pt => new { pt.ProjectId, pt.TagId });

            builder.Entity<ProjectTag>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTags)
                .HasForeignKey(pt => pt.ProjectId);

            builder.Entity<ProjectTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProjectTags)
                .HasForeignKey(pt => pt.TagId);

            builder.Entity<UserProject>()
                .HasOne(up => up.UserProfile)
                .WithMany(p => p.Projects)
                .HasForeignKey(up => up.UserProfileId);

            builder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany()
                .HasForeignKey(up => up.ProjectId);

        }
    }
}
