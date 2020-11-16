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
    }
}
