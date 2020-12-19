using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenLibrary.Models;

namespace RavenLibrary.Raven.Indexes
{
    public class Books_ByUser : AbstractIndexCreationTask<UserBook, Books_ByUser.Result>
    {
        public class Result
        {
            public string UserId { get; set; }

            public string BookId { get; set; }
            
            public int Count { get; set; }
        }

        public Books_ByUser()
        {
            Map = userBooks => from userBook in userBooks
                select new
                {
                    UserId = userBook.UserId,
                    BookId = userBook.BookId,
                    Count = 1
                };

            Reduce = results => from result in results
                group result by result.UserId into g
                select new
                {
                    UserId = g.Key,
                    BookId = "",
                    Count = g.Sum(x => x.Count)
                };
        }
    }
}
