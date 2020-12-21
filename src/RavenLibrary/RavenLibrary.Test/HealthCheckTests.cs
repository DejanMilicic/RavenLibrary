using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RavenLibrary.Models;
using RavenLibrary.Test.Infrastructure;
using Xunit;

namespace RavenLibrary.Test
{
    public class HealthCheckTests : Fixture
    {
        public HealthCheckTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task HealthCheck_ReturnsOK()
        {
            var response = await HttpClient.GetAsync("/healthcheck");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAnnotations_Returns1()
        {
            var session = Store.OpenSession();

            Annotation a = new Annotation
            {
                at = DateTimeOffset.UtcNow,
                start = 123,
                text = "this is my text about note",
                user = "user/123",
                book = "book/123"
            };

            session.Store(a);
            session.SaveChanges();

            List<Annotation> dbAnnotations = session.Query<Annotation>().ToList();

            dbAnnotations.Count.Should().Be(1);

            string aid = dbAnnotations.Single().Id;

            var response = await HttpClient.GetFromJsonAsync<Annotation>("/annotation?id=" + aid);

            response.Should().BeEquivalentTo(a);
        }
    }
}
