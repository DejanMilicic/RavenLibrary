using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public Book Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();
            return session.Load<Book>(id);
        }
    }
}
