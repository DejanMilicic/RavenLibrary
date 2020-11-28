using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenLibrary.Models;

namespace RavenLibrary.Raven.Indexes
{
    public class UserBook_ByUser_ByBook : AbstractIndexCreationTask<UserBook>
    {
        public class Result
        {
            public string UserId { get; set; }

            public string BookId { get; set; }
        }

        public UserBook_ByUser_ByBook()
        {
            Map = ubs => ubs
                .Select(ub => new Result
                {
                    UserId = ub.UserId,
                    BookId = ub.BookId
                });
        }
    }
}
