using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public async Task<Book> Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            return await session.LoadAsync<Book>(id);
        }
    }
}
