
using CareerGuidance.Api.Abstractions.Const;
using Microsoft.AspNetCore.Identity;

namespace CareerGuidance.Api.Persistence
{
    public class RoleConfigurations : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData([
                new IdentityRole {
                    Id = DefaultRole.StudentRoleId,
                    Name = DefaultRole.Student,
                    NormalizedName = DefaultRole.Student.ToUpper(),
                    ConcurrencyStamp = DefaultRole.StudentRoleConcurrencyStamp,
                },
                 new IdentityRole {
                    Id = DefaultRole.AdminRoleId,
                    Name = DefaultRole.Admin,
                    NormalizedName = DefaultRole.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRole.AdminRoleConcurrencyStamp,
                },
                new IdentityRole {
                    Id = DefaultRole.InstructorRoleId,
                    Name = DefaultRole.Instructor,
                    NormalizedName = DefaultRole.Instructor.ToUpper(),
                    ConcurrencyStamp = DefaultRole.InstructorRoleConcurrencyStamp,
                }
                ]);
        }
    }
}
