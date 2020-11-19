using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            public string UserBookId { get; set; }

            public string Text { get; set; }

            public long Start { get; set; }

            public string Note { get; set; }
        }

        [HttpPost]
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
    }
}
