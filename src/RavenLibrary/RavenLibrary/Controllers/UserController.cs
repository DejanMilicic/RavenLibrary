using Microsoft.AspNetCore.Mvc;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public User Get(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenSession();
            return session.Load<User>(id);
        }
    }
}
