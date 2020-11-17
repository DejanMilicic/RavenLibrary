using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavenLibrary.Models
{
    public class Annotation
    {
        public string Id { get; set; }

        public string UserBook { get; set; }

        public string HighlightedText { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string NoteText { get; set; }

        public Location Location { get; set; }
    }

    public class Location
    {
        public string StartPath { get; set; }

        public string EndPath { get; set; }

        public long StartChar { get; set; }

        public long EndChar { get; set; }

        public string ChapterTitle { get; set; }

        public string ChapterFileName { get; set; }

        public double ChapterProgress { get; set; }
    }
}
