using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RavenLibrary.Models;
using RavenLibrary.Test.Infrastructure;
using Xunit;

namespace RavenLibrary.Test.Controllers.UserController
{
    public class UserControllerTests : Fixture
    {
        public UserControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        private class CreateUserModel
        {
            public CreateUserModel(string name, int karmaComments, int karmaLinks)
            {
                Name = name;
                Karma_Comments = karmaComments;
                Karma_Links = karmaLinks;
            }

            public string Name { get; set; }

            public int Karma_Comments { get; set; }

            public int Karma_Links { get; set; }
        }

        [Fact]
        public async Task UserPost_CreatesOneUser()
        {
            var newUser = new CreateUserModel("John Doe", 4, 5);
            var newUserContent = JsonSerializer.Serialize(newUser);

            var stringContent = new StringContent(newUserContent, Encoding.UTF8, MediaTypeNames.Application.Json); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            var result = HttpClient.PostAsync("/user", stringContent).Result;

            var session = Store.OpenSession();

            List<User> users = session.Query<User>().ToList();

            users.Count.Should().Be(1);

            WaitForUserToContinueTheTest(Store);

            var user = users.Single();
            user.Name.Should().Be(newUser.Name);
            user.Karma.Links.Should().Be(newUser.Karma_Links);
            user.Karma.Comments.Should().Be(newUser.Karma_Comments);
        }
    }
}
