using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        [HttpGet("/book")]
        public async Task<Book> GetBook(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            return await session.LoadAsync<Book>(id);
        }

        [HttpGet("/books/user/{userId}/")]
        public async Task<IEnumerable<Book>> GetUserBooks(string userId)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            List<UserBook> userBooks = await session
                .Query<UserBook>().Include(x => x.BookId).ToListAsync();

            Dictionary<string, Book> books = await session.LoadAsync<Book>(userBooks.Select(x => x.BookId));
            return books.Values.ToList();
        }

        [HttpGet("/books/user/{userId}/{skip}/{take}")]
        public async Task<IEnumerable<Book>> GetUserBooksRange(string userId, int skip, int take)
        {
            // todo implement
            return new List<Book>();
        }
    }
}
