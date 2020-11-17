using System;
using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnnotationController : ControllerBase
    {
        [HttpGet]
        public Annotation Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();
            return session.Load<Annotation>(id);
        }

        public class CreateAnnotationModel
        {
            public string UserBook { get; set; }
            public string HighlightedText { get; set; }
            public string NoteText { get; set; }
            public string StartPath { get; set; }
            public string EndPath { get; set; }
            public long StartChar { get; set; }
            public long EndChar { get; set; }
            public string ChapterTitle { get; set; }
            public string ChapterFileName { get; set; }
            public double ChapterProgress { get; set; }
        }

        [HttpPost]
        public string Post([FromBody] CreateAnnotationModel a)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();

            Annotation annotation = new Annotation
            {
                UserBook = a.UserBook,
                HighlightedText = a.HighlightedText,
                NoteText = a.NoteText,
                CreatedAt = DateTimeOffset.UtcNow,
                Location = new Location
                {
                    StartPath = a.StartPath, 
                    EndPath = a.EndPath, 
                    StartChar = a.StartChar,
                    EndChar = a.EndChar,
                    ChapterTitle = a.ChapterTitle,
                    ChapterFileName = a.ChapterFileName,
                    ChapterProgress = a.ChapterProgress
                }
            };


            session.Store(annotation);
            session.SaveChanges();

            return annotation.Id;

        }
    }
}
