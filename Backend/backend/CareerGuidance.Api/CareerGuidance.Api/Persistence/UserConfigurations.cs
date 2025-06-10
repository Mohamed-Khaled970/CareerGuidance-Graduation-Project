using CareerGuidance.Api.Abstractions.Const;

namespace CareerGuidance.Api.Persistence
{
    /*
     * File Name: UserConfigurations.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - using Microsoft.EntityFrameworkCore.Metadata.Builders; // For EntityTypeBuilder
     *      - using Microsoft.EntityFrameworkCore; // For IEntityTypeConfiguration
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     */

    public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        /*
         * Method: Configure
         * Description: Configures the entity properties for ApplicationUser.
         * Parameters:
         *      - builder: The builder used to configure the entity properties.
         */
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Set maximum length for the Name property
            builder.Property(x => x.Name).HasMaxLength(100);

            // Set maximum length for the UserName property
            builder.Property(x => x.UserName).HasMaxLength(100);

            builder
                .OwnsMany(x => x.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(new ApplicationUser
            {
                Id = DefaultUsers.AdminId,
                UserName = DefaultUsers.AdminUsername,
                NormalizedUserName = DefaultUsers.AdminUsername.ToUpper(),
                Email = DefaultUsers.AdminEmail,
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                EmailConfirmed = true,
                Role="Admin",
                PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
            });
        }

    }
}