using System;

namespace RavenLibrary.Models
{
    public class UserBook
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string BookId { get; set; }

        public DateTimeOffset Created { get; set; }

        public int Stars { get; set; }
    }
}
