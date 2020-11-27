using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using RavenLibrary.Models;

namespace RavenLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public AuthorController(IAsyncDocumentSession session)
        {
            _session = session;
        }

        [HttpGet("/author")]
        public async Task<Author> Get(string id)
        {
            return await _session.LoadAsync<Author>(id);
        }
    }
}
