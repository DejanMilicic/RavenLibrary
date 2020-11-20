using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("/user")]
        public async Task<User> Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            return await session.LoadAsync<User>(id);
        }
    }
}
