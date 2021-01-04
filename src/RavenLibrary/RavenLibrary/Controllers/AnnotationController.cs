using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;
using RavenLibrary.Raven.Indexes;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnnotationController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public AnnotationController(IAsyncDocumentSession session, IDocumentSession s)
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
            public string UserId { get; set; }
            
            public string BookId { get; set; }

            public string Text { get; set; }

            public long Start { get; set; }

            public string Note { get; set; }
        }

        [HttpPost("/annotation")]
        public async Task<string> Post([FromBody] CreateAnnotationModel a)
        {
            Annotation annotation = new Annotation
            {
                user = a.UserId,
                book = a.BookId,
                text = a.Text,
                start = a.Start,
                at = DateTimeOffset.UtcNow // todo parametrize
            };

            await _session.StoreAsync(annotation);
            await _session.SaveChangesAsync();

            return annotation.Id;
        }

        [HttpGet("/annotations/user/")]
        public async Task<IEnumerable<Annotation>> GetUserAnnotations(string userId)
        {
            return await _session
                .Query<Annotations_ByUser.Result, Annotations_ByUser>()
                .Where(x => x.UserId == userId)
                .OfType<Annotation>()
                .ToArrayAsync();
        }

        public class GetAnnotationsRangeResponse
        {
            public IEnumerable<Annotation> AnnotationsPage { get; set; }

            public int Total { get; set; }
        }

        [HttpGet("/annotations/user/{skip}/{take}")]
        public async Task<GetAnnotationsRangeResponse> GetUserAnnotationsRange(string userId, int skip, int take)
        {
            var userAnnotations = await _session
                .Query<Annotations_ByUser.Result, Annotations_ByUser>()
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Where(x => x.UserId == userId)
                .OfType<Annotation>()
                .ToArrayAsync();

            return new GetAnnotationsRangeResponse
            {
                AnnotationsPage = userAnnotations,
                Total = stats.TotalResults
            };
        }

        [HttpGet("/annotations/")]
        public async IAsyncEnumerable<Annotation> GetUserBookAnnotations(string userId, string bookId)
        {
            var query = _session
                .Query<Annotation>()
                .Where(x => x.Id.StartsWith($"Annotations/{userId}-{bookId}/"));

            await using var stream = await _session.Advanced.StreamAsync(query);

            while (await stream.MoveNextAsync())
                yield return stream.Current.Document;
        }

        [HttpGet("/annotations/{skip}/{take}")]
        public async Task<GetAnnotationsRangeResponse> GetUserBookAnnotations(string userId, string bookId, int skip, int take)
        {
            var userBookAnnotations = await _session
                .Query<Annotation>()
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Where(x => x.Id.StartsWith($"Annotations/{userId}-{bookId}/"))
                .ToArrayAsync();

            return new GetAnnotationsRangeResponse
            {
                AnnotationsPage = userBookAnnotations,
                Total = stats.TotalResults
            };
        }

        [HttpGet("/annotations/userbook/")]
        public async Task<IEnumerable<Annotation>> GetUserBookAnnotations(string userBookId)
        {
            return await _session
                .Query<Annotation>()
                .Where(x => x.Id.StartsWith($"Annotations/{userBookId}/"))
                .ToArrayAsync();
        }

        [HttpGet("/annotations/userbook/{skip}/{take}")]
        public async Task<GetAnnotationsRangeResponse> GetUserBookAnnotations(string userBookId, int skip, int take)
        {
            var userBookAnnotations = await _session
                .Query<Annotation>()
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Where(x => x.Id.StartsWith($"Annotations/{userBookId}/"))
                .ToArrayAsync();

            return new GetAnnotationsRangeResponse
            {
                AnnotationsPage = userBookAnnotations,
                Total = stats.TotalResults
            };
        }

        [HttpGet("/annotations/user/after/")]
        public async Task<IEnumerable<Annotation>> GetUserAnnotationsAfter(string userId, DateTimeOffset after)
        {
            // todo implement
            // will see if this needs to be implemented, or maybe not
            return new List<Annotation>();
        }

        [HttpGet("/annotations/user/book/after/")]
        public async Task<IEnumerable<Annotation>> GetUserBookAnnotationsAfter(string userId, string bookId, DateTimeOffset after)
        {
            // todo implement
            return new List<Annotation>();
        }
    }
}
