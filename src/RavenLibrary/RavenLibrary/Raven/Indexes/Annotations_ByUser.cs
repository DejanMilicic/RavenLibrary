using System;
using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenLibrary.Models;

namespace RavenLibrary.Raven.Indexes
{
    public class Annotations_ByUser : AbstractIndexCreationTask<Annotation>
    {
        public class Result
        {
            public string UserId { get; set; }

            public string AnnotationId { get; set; }
        }

        public Annotations_ByUser()
        {
            Map = ans => ans
                .Select(a => new Result
                {
                    AnnotationId = a.Id,
                    UserId = a.UserBookId
                        .Replace("UsersBooks/", "") // 
                        .Split(new string[]{ "-books", "-ebooks" }, StringSplitOptions.RemoveEmptyEntries)[0]
                });
        }
    }
}
