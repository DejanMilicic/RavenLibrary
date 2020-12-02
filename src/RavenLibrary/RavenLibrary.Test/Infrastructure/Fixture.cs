using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.TestDriver;
using RavenLibrary.Raven.Indexes;
using Xunit;

namespace RavenLibrary.Test.Infrastructure
{
    public class Fixture : RavenTestDriver, IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient HttpClient;
        protected readonly IDocumentStore Store;

        public Fixture(WebApplicationFactory<Startup> factory)
        {
            Store = this.GetDocumentStore();
            IndexCreation.CreateIndexes(typeof(UserBook_ByUser_ByBook).Assembly, Store);

            HttpClient = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IDocumentStore>(Store);
                    });
                })
                .CreateDefaultClient();
        }
    }
}
