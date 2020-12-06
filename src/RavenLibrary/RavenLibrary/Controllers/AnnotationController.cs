﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;
using RavenLibrary.Raven.Indexes;
using RavenLibrary.Shared;

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

        [HttpGet("/annotations/user/")]
        public async Task<IEnumerable<Annotation>> GetUserAnnotations(string userId)
        {
            return await _session
                .Query<Annotation, Annotations_ByUserBook>()
                .Where(x => x.UserBookId.StartsWith(Util.GetUserBookCollectionUserPrefix(userId)))
                .ToArrayAsync();
        }

        public class GetUserAnnotationsRangeResponse
        {
            public IEnumerable<Annotation> AnnotationsPage { get; set; }

            public int Total { get; set; }
        }

        [HttpGet("/annotations/user/{skip}/{take}")]
        public async Task<GetUserAnnotationsRangeResponse> GetUserAnnotationsRange(string userId, int skip, int take)
        {
            var userAnnotations = await _session
                .Query<Annotation, Annotations_ByUserBook>()
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Where(x => x.UserBookId.StartsWith(Util.GetUserBookCollectionUserPrefix(userId)))
                .ToArrayAsync();

            return new GetUserAnnotationsRangeResponse
            {
                AnnotationsPage = userAnnotations,
                Total = stats.TotalResults
            };
        }

        [HttpGet("/annotations/userbook/")]
        public async Task<IEnumerable<Annotation>> GetUserBookAnnotations(string userBookId)
        {
            return await _session
                .Query<Annotation, Annotations_ByUserBook>()
                .Where(x => x.UserBookId == userBookId)
                .ToArrayAsync();
        }

        [HttpGet("/annotations/")]
        public async Task<IEnumerable<Annotation>> GetAnnotationsForUserForBook(string userId, string bookId)
        {
            return await _session
                .Query<Annotation, Annotations_ByUserBook>()
                .Where(x => x.UserBookId.StartsWith(Util.GetUserBookCollection(userId, bookId)))
                .ToArrayAsync();
        }

        [HttpGet("/annotations/{skip}/{take}")]
        public async Task<GetUserAnnotationsRangeResponse> GetAnnotationsRangeForUserForBook(string userId, string bookId, int skip, int take)
        {
            var userBookAnnotations = await _session
                .Query<Annotation, Annotations_ByUserBook>()
                .Skip(skip)
                .Take(take)
                .Statistics(out QueryStatistics stats)
                .Where(x => x.UserBookId.StartsWith(Util.GetUserBookCollection(userId, bookId)))
                .ToArrayAsync();

            return new GetUserAnnotationsRangeResponse
            {
                AnnotationsPage = userBookAnnotations,
                Total = stats.TotalResults
            };
        }

        [HttpGet("/annotations/user/after/")]
        public async Task<IEnumerable<Annotation>> GetUserAnnotationsAfter(string userId, DateTimeOffset after)
        {
            // todo implement
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