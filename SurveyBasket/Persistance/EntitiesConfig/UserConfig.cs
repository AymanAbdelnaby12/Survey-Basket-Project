using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Persistance.EntitiesConfig
{
    public class UserConfig:IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName)
                .HasMaxLength(100);
            builder.Property(x => x.LastName)
                .HasMaxLength(100);
        }
    } 
}
