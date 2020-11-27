using System;
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
    public class AnnotationController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public AnnotationController(IAsyncDocumentSession session)
        {
            _session = session;
        }

        [HttpGet("/annotation")]
        public async Task<Annotation> Get(string id)
        {
            return await _session.LoadAsync<Annotation>(id);
        }

        public class CreateAnnotationModel
        {
            public string UserBookId { get; set; }

            public string Text { get; set; }

            public long Start { get; set; }

            public string Note { get; set; }
        }

        [HttpPost("/annotation")]
        public async Task<string> Post([FromBody] CreateAnnotationModel a)
        {
            Annotation annotation = new Annotation
            {
                UserBookId = a.UserBookId,
                Text = a.Text,
                Start = a.Start,
                Note = a.Note,
                Created = DateTimeOffset.UtcNow
            };


            await _session.StoreAsync(annotation);
            await _session.SaveChangesAsync();

            return annotation.Id;

        }

        [HttpGet("/annotations/userbook/")]
        public async Task<IEnumerable<Annotation>> GetUserBookAnnotations(string userBookId)
        {
            return await _session
                .Query<Annotation>()
                .Where(x => x.UserBookId == userBookId)
                .ToArrayAsync();
        }

        [HttpGet("/annotations/")]
        public async Task<IEnumerable<Annotation>> GetAnnotationsForUserForBook(string userId, string bookId)
        {
            UserBook userBook = await _session
                .Query<UserBook>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (userBook == null) return Enumerable.Empty<Annotation>();

            return await _session
                .Query<Annotation>()
                .Where(x => x.UserBookId == userBook.Id)
                .ToArrayAsync();
        }

        [HttpGet("/annotations/user/{userId}/after/{after}")]
        public async Task<IEnumerable<Book>> GetUserAnnotationsAfter(string userId, DateTimeOffset after)
        {
            // todo implement
            return new List<Book>();
        }

        [HttpGet("/annotations/user/{userId}/book/{bookId}")]
        public async Task<IEnumerable<Book>> GetUserBookAnnotations(string userId, string bookId)
        {
            // todo implement
            return new List<Book>();
        }

        [HttpGet("/annotations/user/{userId}/book/{bookId}/after/{after}")]
        public async Task<IEnumerable<Book>> GetUserBookAnnotationsAfter(string userId, string bookId, DateTimeOffset after)
        {
            // todo implement
            return new List<Book>();
        }
    }
}
