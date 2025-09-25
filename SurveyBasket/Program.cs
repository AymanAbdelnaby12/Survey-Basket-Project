using Microsoft.EntityFrameworkCore;
using SurveyBasket.Controllers;
using SurveyBasket.Persistance;

namespace SurveyBasket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependencies(builder.Configuration); 
            builder.Services.AddSwaggerServices(); 

            var app = builder.Build(); 
            app.MapOpenApi(); 
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();       
            app.UseAuthentication();
            app.UseAuthorization(); 
            app.MapControllers();


            app.Run();
        }
    }
}
