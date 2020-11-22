using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
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

        [HttpGet("/books/user/")]
        public async Task<IEnumerable<Book>> GetUserBooks(string userId)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            List<UserBook> userBooks = await session
                .Query<UserBook>()
                .Where(x => x.UserId == userId)
                .Include(x => x.BookId).ToListAsync();

            Dictionary<string, Book> books = await session.LoadAsync<Book>(userBooks.Select(x => x.BookId));
            return books.Values.ToList();
        }

        public class GetUserBooksRangeResponse
        {
            public IEnumerable<Book> BooksPage { get; set; }

            public int BooksTotal { get; set; }
        }

        [HttpGet("/books/user/{skip}/{take}")]
        public async Task<GetUserBooksRangeResponse> GetUserBooksRange(string userId, int skip, int take)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            List<UserBook> userBooks = await session
                .Query<UserBook>()
                .Where(x => x.UserId == userId)
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Include(x => x.BookId).ToListAsync();

            Dictionary<string, Book> books = await session.LoadAsync<Book>(userBooks.Select(x => x.BookId));
            return new GetUserBooksRangeResponse
            {
                BooksPage = books.Values,
                BooksTotal = stats.TotalResults
            };
        }
    }
}
