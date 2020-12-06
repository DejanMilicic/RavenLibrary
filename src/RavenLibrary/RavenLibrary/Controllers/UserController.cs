using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public UserController(IAsyncDocumentSession session)
        {
            _session = session;
        }

        [HttpGet("/user")]
        public async Task<User> Get(string id)
        {
            return await _session.LoadAsync<User>(id);
        }

        [HttpGet("/user/random")]
        public async Task<User> GetRandom()
        {
            var cs = await _session.Advanced.DocumentStore
                .Maintenance.SendAsync(new GetCollectionStatisticsOperation());

            int totalUsersCount = (int)cs.Collections["Users"];

            Random r = new Random();
            int randomUserOrdinal = r.Next(1, totalUsersCount)-1;

            var randomUser = await _session.Query<User>()
                .Skip(randomUserOrdinal)
                .Take(1).SingleAsync();

            return randomUser;
        }

        public class CreateUserModel
        {
            public string Name { get; set; }

            public int Karma_Comments { get; set; }

            public int Karma_Links { get; set; }
        }

        [HttpPost("/user")]
        public async Task<string> Post([FromBody] CreateUserModel u)
        {
            User user = new User
            {
                Name = u.Name,
                Karma = new Karma
                {
                    Comments = u.Karma_Comments,
                    Links = u.Karma_Links
                },
                Created = DateTimeOffset.UtcNow
            };

            await _session.StoreAsync(user);
            await _session.SaveChangesAsync();

            return user.Id;
        }
    }
}
