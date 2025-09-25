using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace SurveyBasket.Persistance.EntitiesConfig
{
    public class PollConfig:IEntityTypeConfiguration<Poll>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(x => x.Title)
                .IsUnique(); 

            builder.Property(x => x.Title)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(1500);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

        }
    } 
}
