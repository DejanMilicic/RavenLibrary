using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;
using RavenLibrary.Shared;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserBookController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public UserBookController(IAsyncDocumentSession session)
        {
            _session = session;
        }

        [HttpGet("/userbook")]
        public async Task<UserBook> Get(string id)
        {
            return await _session.LoadAsync<UserBook>(id);
        }

        public class CreateUserBookModel
        {
            public string User { get; set; }

            public string Book { get; set; }
        }

        [HttpPost("/userbook")]
        public async Task<string> Post([FromBody] CreateUserBookModel ub)
        {
            UserBook userBook = new UserBook
            {
                Id = Util.GetUserBookCollection(ub.User, ub.Book),
                UserId = ub.User,
                BookId = ub.Book,
                Stars = 0,
                Created = DateTimeOffset.UtcNow
            };


            await _session.StoreAsync(userBook);
            await _session.SaveChangesAsync();

            return userBook.Id;
        }

        [HttpGet("/userbook/random")]
        public async Task<UserBook> GetRandom()
        {
            var cs = await _session.Advanced.DocumentStore
                .Maintenance.SendAsync(new GetCollectionStatisticsOperation());

            int totalUserBookCount = (int)cs.Collections["UserBooks"];

            Random r = new Random();
            int randomUserBookOrdinal = r.Next(1, totalUserBookCount) - 1;

            var randomBook = await _session.Query<UserBook>()
                .Skip(randomUserBookOrdinal)
                .Take(1).SingleAsync();

            return randomBook;
        }
    }
}
