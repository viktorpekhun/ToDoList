
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Infrastructure.Options;
using ToDoList.Infrastructure.Persistence;

namespace ToDoList.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var dbOptions = new DatabaseOptions();
                configuration.GetSection(DatabaseOptions.SectionName).Bind(dbOptions);

                if (string.IsNullOrWhiteSpace(dbOptions.User) || string.IsNullOrWhiteSpace(dbOptions.Password))
                {
                    throw new InvalidOperationException(
                        "Database settings are empty.");
                }

                var connectionString = dbOptions.ToConnectionString();

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }

}
