using CareerGuidance.Api.Models;
using System;

namespace CareerGuidance.Api.DbContext
{
    /*
     * File Name: applicationDbContext.cs
     * Author Information: Mohamed Khaled , Abdelrahman Rezk
     * Date of creation: 2024-08-19
     * Versions Information: v1.0
     * Dependencies:
     *      - Microsoft.AspNetCore.Http
     *      - Microsoft.EntityFrameworkCore
     *      - Microsoft.AspNetCore.Identity
     *      - System.Security.Claims
     *      - System.Reflection
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This class represents the application's database context for Entity Framework.
     *      It extends IdentityDbContext to include user management functionalities and
     *      includes DbSet properties for entities used in the application.
     *      It also manages auditing information by tracking who created or modified an entity
     *      and when those changes occurred.
     */
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser , IdentityRole,string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<allRoadmapInserted> Roadmaps { get; set; }


        //public DbSet<InformationHomePageForRoadmap> InformationRoadmap { get; set; }
        public DbSet<roadmapCategory> RoadmapCategories  { get; set; }
        public DbSet<IntroductionHomePage> IntroductionHomePage { get; set; } 


        public DbSet<Roadmap> ParsedRoadmaps { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Edge> Edges { get; set; }

        public DbSet<DetailsCarouselSection> DetailsCarouselSection { get; set; }
        public DbSet<NewCarouselSection> NewCarouselSection { get; set; }
        public DbSet<IntroductionStartHerePage> IntroductionStartHere { get; set; } 
        public DbSet<ImportantStartHere> ImportantStartHere { get; set; }

        public DbSet<socialMedia> socialMedia { get; set; }
        public DbSet<progressBar> progressBar { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewApplication> InterviewApplications { get; set; }
        public DbSet<InterviewerExperience> InterviewersExperience { get; set; }



        // Configuring model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Roadmap>()
                .HasMany(r => r.Nodes)
                .WithOne(n => n.Roadmap)
                .HasForeignKey(n => n.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Node>()
                .HasMany(n => n.Links)
                .WithOne(l => l.Node)
                .HasForeignKey(l => l.NodeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Roadmap>()
                .HasMany(r => r.Edges)
                .WithOne(e => e.Roadmap)
                .HasForeignKey(e => e.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<NewCarouselSection>()
               .HasMany(r => r.DetailsCarouselSection) //name relation Many
               .WithOne(n => n.CarouselSection) // name one
               .HasForeignKey(n => n.CarouselSectionId) //name FK
               .OnDelete(DeleteBehavior.Cascade);  // Delete Type

            /*****************************/
            modelBuilder.Entity<socialMedia>()
            .HasOne(s => s.ApplicationUser)
            .WithOne(u => u.SocialMedia)
            .HasForeignKey<socialMedia>(s => s.ApplicationUserId);
            /*****************************/
            modelBuilder.Entity<progressBar>()
            .HasOne(p => p.ApplicationUser)
            .WithMany(u => u.ProgressBars)
            .HasForeignKey(p => p.ApplicationUserId) // تأكد إن اسم الـ FK صح
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Roadmap>()
            .HasMany(r => r.ProgressBar)
            .WithOne(pb => pb.Roadmap)
            .HasForeignKey(pb => pb.RoadmapId)
            .OnDelete(DeleteBehavior.Cascade);  // Delete progressBar when Roadmap is deleted


        }


        /*
         * Override of SaveChangesAsync
         * 
         * This method is overridden to include auditing capabilities.
         */
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Retrieve all tracked entities of type AuditableEntity
            var entries = ChangeTracker.Entries<AuditableEntity>();

            // Iterate through the entities that need auditing
            foreach (var entityEntry in entries)
            {
                // Get the ID of the currently authenticated user
                var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

                // If the entity is being added
                if (entityEntry.State == EntityState.Added)
                {
                    // Set the CreatedById property to the current user's ID
                    entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                }
                // If the entity is being modified
                else if (entityEntry.State == EntityState.Modified)
                {
                    // Set the UpdatedById property to the current user's ID
                    entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                    // Set the UpdatedOn property to the current UTC timestamp
                    entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }

            // Save all changes made to the context and return the result
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}