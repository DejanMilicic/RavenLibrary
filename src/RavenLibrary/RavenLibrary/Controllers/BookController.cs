using System;
using System.Collections;
using System.Collections.Generic;
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

        [HttpGet("/GetAll")]
        public async Task<IEnumerable<Book>> GetAll(string userId)
        {
            // todo implement
            return new List<Book>();
        }

        [HttpGet("/GetRange")]
        public async Task<IEnumerable<Book>> GetRange(string userId, int skip, int take)
        {
            // todo implement
            return new List<Book>();
        }
    }
}
