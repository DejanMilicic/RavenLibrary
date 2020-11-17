using System;

namespace RavenLibrary.Models
{
    public class UserBook
    {
        public string Id { get; set; }

        public string text { get; set; }

        public string book { get; set; }

        public string user { get; set; }

        public long start { get; set; }

        public DateTimeOffset at { get; set; }
    }
}
