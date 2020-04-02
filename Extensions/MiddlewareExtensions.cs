using Domain.Persistence.Contexts;
using Domain.Persistence.Repositories;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Supermarket API";
                    document.Info.Description = "A simple ASP.NET Core web API 2";
                    document.Info.TermsOfService = "https://example.com/terms";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Eric Perez Villar",
                        Email = string.Empty,
                        Url = "https://www.linkedin.com/in/eric-perez-villar-87aa6746/",
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };

            });
            return services;
        }

        public static IServiceCollection AddScopeCustom(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("Supermarket.API-in-memory");
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Supermarket swagger");
            });
            app.UseOpenApi();
            app.UseSwaggerUi3();
            return app;
        }
    }
}
