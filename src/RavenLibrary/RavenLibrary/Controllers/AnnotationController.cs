﻿using System;
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
    public class AnnotationController : ControllerBase
    {
        [HttpGet("/annotation")]
        public Annotation Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();
            return session.Load<Annotation>(id);
        }

        public class CreateAnnotationModel
        {
            public string UserBookId { get; set; }

            public string Text { get; set; }

            public long Start { get; set; }

            public string Note { get; set; }
        }

        [HttpPost("/annotation")]
        public string Post([FromBody] CreateAnnotationModel a)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();

            Annotation annotation = new Annotation
            {
                UserBookId = a.UserBookId,
                Text = a.Text,
                Start = a.Start,
                Note = a.Note,
                Created = DateTimeOffset.UtcNow
            };


            session.Store(annotation);
            session.SaveChanges();

            return annotation.Id;

        }

        [HttpGet("/annotations/userbook/")]
        public async Task<IEnumerable<Annotation>> GetUserBookAnnotations(string userBookId)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();

            return await session
                .Query<Annotation>()
                .Where(x => x.UserBookId == userBookId)
                .ToArrayAsync();
        }

        [HttpGet("/annotations/")]
        public async Task<IEnumerable<Annotation>> GetAnnotationsForUserForBook(string userId, string bookId)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();

            UserBook userBook = await session
                .Query<UserBook>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (userBook == null) return Enumerable.Empty<Annotation>();

            return await session
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
