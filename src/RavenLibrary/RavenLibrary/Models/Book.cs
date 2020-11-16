using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavenLibrary.Models
{
    public class Book
    {
        public string Id { get; set; }

        public List<string> subject { get; set; }

        public List<string> language { get; set; }

        public List<string> bookshelf { get; set; }

        public string tableOfContents { get; set; }

        public int downloads { get; set; }

        public List<string> creator { get; set; }

        public string publisher { get; set; }

        public DateTimeOffset issued { get; set; }

        public string title { get; set; }

        public string rights { get; set; }

        public string type { get; set; }

        // TODO: fix handling of this property
        //public Uri en.wikipedia { get; set; }
    }
}
