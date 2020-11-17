using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        [HttpGet]
        public async Task<Author> Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            return await session.LoadAsync<Author>(id);
        }
    }
}
