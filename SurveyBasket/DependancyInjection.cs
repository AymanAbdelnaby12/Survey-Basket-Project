using FluentValidation;
using Mapster;
using MapsterMapper;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Services;
using System.Reflection;

namespace SurveyBasket
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services)
        {
            // Add services to the container
           services.AddControllers();
           services.AddOpenApi();
           services.AddScoped<IPollService, PollService>();

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
