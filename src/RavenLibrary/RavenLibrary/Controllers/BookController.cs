using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;
using RavenLibrary.Raven.Indexes;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public BookController(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public class CreateBookModel
        {
            public List<string> Subject { get; set; }

            public List<string> Language { get; set; }

            public List<string> Bookshelf { get; set; }

            public string TableOfContents { get; set; }

            public int Downloads { get; set; }

            public List<string> Creator { get; set; }

            public string Publisher { get; set; }

            public string Issued { get; set; }

            public string Title { get; set; }

            public string Rights { get; set; }

            public string Type { get; set; }
        }

        [HttpPost("/book")]
        public async Task<string> Post([FromBody] CreateBookModel b)
        {
            Book book = new Book
            {
                subject = b.Subject,
                language = b.Language,
                bookshelf = b.Bookshelf,
                tableOfContents = b.TableOfContents,
                downloads = b.Downloads,
                creator = b.Creator,
                publisher = b.Publisher,
                issued = b.Issued,
                title = b.Title,
                rights = b.Rights,
                type = b.Type
            };

            await _session.StoreAsync(book);
            await _session.SaveChangesAsync();

            return book.Id;
        }

        [HttpGet("/book")]
        public async Task<Book> GetBook(string id)
        {
            return await _session.LoadAsync<Book>(id);
        }

        [HttpGet("/books/user/")]
        public async Task<IEnumerable<Book>> GetUserBooks(string userId)
        {
            var userBooks = await _session
                .Query<UserBook_ByUser_ByBook.Result, UserBook_ByUser_ByBook>()
                .Where(x => x.UserId == userId)
                .Include(x => x.BookId)
                .ToArrayAsync();

            Dictionary<string, Book> books = await _session.LoadAsync<Book>(userBooks.Select(x => x.BookId));
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
            var userBooks = await _session
                .Query<UserBook_ByUser_ByBook.Result, UserBook_ByUser_ByBook>()
                .Where(x => x.UserId == userId)
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Include(x => x.BookId)
                .ToArrayAsync();

            Dictionary<string, Book> books = await _session.LoadAsync<Book>(userBooks.Select(x => x.BookId));

            return new GetUserBooksRangeResponse
            {
                BooksPage = books.Values,
                BooksTotal = stats.TotalResults
            };
        }

        [HttpGet("/books/random")]
        public async Task<Book> GetRandom()
        {
            return await _session.Query<Book>()
                .Customize(x => x.RandomOrdering())
                .Take(1).SingleAsync();
        }
    }
}
