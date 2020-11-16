using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        [HttpGet]
        public Author Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();
            return session.Load<Author>(id);
        }
    }
}
