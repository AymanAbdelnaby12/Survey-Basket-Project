using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Persistance.EntitiesConfig
{
    public class PollConfig:IEntityTypeConfiguration<Poll>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(1500); 
        }
    } 
}
