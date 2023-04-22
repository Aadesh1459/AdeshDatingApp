using AdeshDatingApp.Data;
using AdeshDatingApp.Interface;
using AdeshDatingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace AdeshDatingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
                {
                options.AddDefaultPolicy(policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                });

            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}