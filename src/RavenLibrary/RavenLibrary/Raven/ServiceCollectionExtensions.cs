using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;

namespace RavenLibrary.Raven
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRavenDocumentStore(this IServiceCollection services)
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Library"
            };

            store.Initialize();

            services.AddSingleton<IDocumentStore>(store);

            return services;
        }

        public static IServiceCollection AddRavenAsyncSession(this IServiceCollection services)
        {
            return services.AddScoped(serviceProvider => serviceProvider.GetService<IDocumentStore>()?.OpenAsyncSession());
        }
    }
}
