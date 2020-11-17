using System;
using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserBookController : ControllerBase
    {
        [HttpGet]
        public UserBook Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();
            return session.Load<UserBook>(id);
        }

        public class CreateUserBookModel
        {
            public string User { get; set; }
            public string Book { get; set; }
            public string Text { get; set; }
            public long Start { get; set; }
        }

        [HttpPost]
        public string Post([FromBody] CreateUserBookModel ub)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();

            UserBook userBook = new UserBook
            {
                Id = $"UsersBooks/{ub.User}-{ub.Book}/",
                text = ub.Text,
                book = ub.Book,
                user = ub.User,
                start = ub.Start,
                at = DateTimeOffset.UtcNow
            };


            session.Store(userBook);
            session.SaveChanges();

            return userBook.Id;
        }
    }
}
