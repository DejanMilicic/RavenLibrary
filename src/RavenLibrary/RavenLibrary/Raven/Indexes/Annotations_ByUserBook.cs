using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenLibrary.Models;

namespace RavenLibrary.Raven.Indexes
{
    public class Annotations_ByUserBook : AbstractIndexCreationTask<Annotation>
    {
        public class Result
        {
            public string UserBookId { get; set; }
        }

        public Annotations_ByUserBook()
        {
            Map = ans => ans
                .Select(a => new Result
                {
                    UserBookId = a.UserBookId
                });
        }
    }
}
