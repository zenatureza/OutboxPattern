using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OutboxPattern.Persistence.Interceptors;

namespace OutboxPattern.Persistence
{
    public static class PersistenceConfig
    {
        public static void AddDbServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddSingleton<DomainEventToOutboxMessageInterceptor>();

            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var interceptor = sp.GetService<DomainEventToOutboxMessageInterceptor>();

                var connectionString = configuration.GetConnectionString("Default");

                options.UseSqlServer(connectionString)
                    .AddInterceptors(interceptor);
            });
        }
    }
}
