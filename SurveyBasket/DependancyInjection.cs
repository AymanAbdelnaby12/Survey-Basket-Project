using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Persistance;
using SurveyBasket.Services;
using System.Reflection;

namespace SurveyBasket
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container
           services.AddControllers();
           services.AddOpenApi();
           services.AddScoped<IPollService, PollService>();


            // ✅ Add services here
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
              throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // add Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
           services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            // add FluentValidation
           services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
           services.AddFluentValidationAutoValidation();

            return services;

        }
    }
}
