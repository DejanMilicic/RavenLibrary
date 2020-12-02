using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.TestDriver;
using RavenLibrary.Models;
using Xunit;

namespace RavenLibrary.Test
{
    public class HealthCheckTests : RavenTestDriver, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private IDocumentStore _store;

        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _store = this.GetDocumentStore();

            _httpClient = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IDocumentStore>(_store);
                    });
                })
                .CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsOK()
        {
            var response = await _httpClient.GetAsync("/healthcheck");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAnnotations_Returns1()
        {
            var session = _store.OpenSession();

            Annotation a = new Annotation
            {
                Created = DateTimeOffset.UtcNow,
                Note = "this is my note",
                Start = 123,
                Text = "this is my text about note",
                UserBookId = "userBooks/123"
            };

            session.Store(a);
            session.SaveChanges();

            List<Annotation> dbAnnotations = session.Query<Annotation>().ToList();

            dbAnnotations.Count.Should().Be(1);

            string aid = dbAnnotations.Single().Id;

            var response = await _httpClient.GetFromJsonAsync<Annotation>("/annotation?id=" + aid);

            response.Should().BeEquivalentTo(a);
        }
    }
}
