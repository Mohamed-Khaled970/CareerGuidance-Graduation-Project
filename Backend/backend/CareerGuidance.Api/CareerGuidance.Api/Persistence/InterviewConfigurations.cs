namespace CareerGuidance.Api.Persistence
{
    public class InterviewConfigurations : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            // العلاقة بين Interview و ApplicationUser (Interviewer)
            builder.HasOne(i => i.Interviewer)
                   .WithMany()
                   .HasForeignKey(i => i.InterviewerId)
                   .OnDelete(DeleteBehavior.Cascade); // مسموح تمسح الـ interview لو مسحت الـ user

            // العلاقة بين Interview و InterviewApplications
            builder.HasMany(i => i.Applications)
                   .WithOne(a => a.Interview)
                   .HasForeignKey(a => a.InterviewId)
                   .OnDelete(DeleteBehavior.Restrict); // أو Use DeleteBehavior.NoAction
        }
    }
}
