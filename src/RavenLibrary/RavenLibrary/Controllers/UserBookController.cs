using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserBookController : ControllerBase
    {
        [HttpGet]
        public async Task<UserBook> Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            return await session.LoadAsync<UserBook>(id);
        }

        public class CreateUserBookModel
        {
            public string User { get; set; }
            public string Book { get; set; }
            public string Text { get; set; }
            public long Start { get; set; }
        }

        [HttpPost]
        public async Task<string> Post([FromBody] CreateUserBookModel ub)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();

            UserBook userBook = new UserBook
            {
                Id = $"UsersBooks/{ub.User}-{ub.Book}/",
                text = ub.Text,
                book = ub.Book,
                user = ub.User,
                start = ub.Start,
                at = DateTimeOffset.UtcNow
            };


            await session.StoreAsync(userBook);
            await session.SaveChangesAsync();

            return userBook.Id;
        }
    }
}
