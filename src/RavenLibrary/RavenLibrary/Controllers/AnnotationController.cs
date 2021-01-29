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

        public AnnotationController(IAsyncDocumentSession session)
        {
            _session = session;
        }

    
        // [HttpGet("/annotations/user/")]
        // public AsyncQueryResult<Annotation> GetUserAnnotations(string userId)
        // {
        //     var query = _session
        //         .Query<Annotations_ByUser.Result, Annotations_ByUser>()
        //         .Where(x => x.UserId == userId)
        //         .OfType<Annotation>();

        //     return new AsyncQueryResult<Annotation>(_session, query);
        // }

        [HttpGet("/annotations/user/{skip}/{take}")]
        public AsyncQueryResult<Annotation> GetUserAnnotationsRange(string userId, int skip, int take)
        {
            var query = _session.Advanced.AsyncDocumentQuery<Annotation>("Annotations/ByUser")
                .Skip(skip)
                .Take(take)
                .WhereEquals("UserId", userId);
            
            return new AsyncQueryResult<Annotation>(_session, query);
        }

        // [HttpGet("/annotations/")]
        // public AsyncQueryResult<Annotation> GetUserBookAnnotations(string userId, string bookId)
        // {
        //     var query = _session.Advanced.AsyncDocumentQuery<Annotation>()
        //         .WhereStartsWith("id()", $"Annotations/{userId}-{bookId}/");

        //     return new AsyncQueryResult<Annotation>(_session, query);
        // }

        // [HttpGet("/annotations/{skip}/{take}")]
        // public async Task<object> GetUserBookAnnotations(string userId, string bookId, int skip, int take)
        // {
        //     var results = await _session.Advanced.AsyncDocumentQuery<Annotation>()
        //         .WhereStartsWith("id()", $"Annotations/{userId}-{bookId}/")
        //         .Skip(skip)
        //         .Take(take)
        //         .ToListAsync();

        //     return results;
        // }

        
        // [HttpGet("/annotations/userbook/")]
        // public AsyncQueryResult<Annotation> GetUserBookAnnotations(string userBookId)
        // {
        //     var query = _session
        //         .Query<Annotation>()
        //         .Where(x => x.Id.StartsWith($"Annotations/{userBookId}/"));

        //     return new AsyncQueryResult<Annotation>(_session, query);
        // }

        [HttpGet("/annotations/userbook/{skip}/{take}")]
        public AsyncQueryResult<Annotation> GetUserBookAnnotations(string userBookId, int skip, int take)
        {
            var query = _session.Advanced.AsyncDocumentQuery<Annotation>()
                .Skip(skip)
                .Take(take)
                .WhereStartsWith("id()", $"Annotations/{userBookId}/");
            
            return new AsyncQueryResult<Annotation>(_session, query);
        }
    }
}
