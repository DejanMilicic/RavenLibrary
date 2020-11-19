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
        }

        [HttpPost]
        public async Task<string> Post([FromBody] CreateUserBookModel ub)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();

            UserBook userBook = new UserBook
            {
                Id = $"UsersBooks/{ub.User}-{ub.Book}/",
                UserId = ub.User,
                BookId = ub.Book,
                Stars = 0,
                Created = DateTimeOffset.UtcNow
            };


            await session.StoreAsync(userBook);
            await session.SaveChangesAsync();

            return userBook.Id;
        }
    }
}
