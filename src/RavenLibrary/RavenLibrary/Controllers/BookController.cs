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

        [HttpGet("/user/{userId}/")]
        public async Task<IEnumerable<Book>> GetUserAll(string userId)
        {
            // todo implement
            return new List<Book>();
        }

        [HttpGet("/user/{userId}/{skip}/{take}")]
        public async Task<IEnumerable<Book>> GetUserRange(string userId, int skip, int take)
        {
            // todo implement
            return new List<Book>();
        }
    }
}
