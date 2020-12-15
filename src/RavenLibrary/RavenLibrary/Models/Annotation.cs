using System;

namespace RavenLibrary.Models
{
    public class Annotation
    {
        public string Id { get; set; }

        public string UserBookId { get; set; }

        public string Text { get; set; }

        public long Start { get; set; }

        public string Note { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
