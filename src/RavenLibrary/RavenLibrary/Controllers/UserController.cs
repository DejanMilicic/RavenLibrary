using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public UserController(IAsyncDocumentSession session)
        {
            _session = session;
        }

        [HttpGet("/user")]
        public async Task<User> Get(string id)
        {
            return await _session.LoadAsync<User>(id);
        }
    }
}
