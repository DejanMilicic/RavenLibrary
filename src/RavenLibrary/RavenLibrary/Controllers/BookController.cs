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
        [HttpGet("/book")]
        public async Task<Book> GetBook(string id)
        {
            using var session = DocumentStoreHolder.Store.OpenAsyncSession();
            return await session.LoadAsync<Book>(id);
        }

        [HttpGet("/books/user/{userId}/")]
        public async Task<IEnumerable<Book>> GetUserBooks(string userId)
        {
            // todo implement
            return new List<Book>();
        }

        [HttpGet("/books/user/{userId}/{skip}/{take}")]
        public async Task<IEnumerable<Book>> GetUserBooksRange(string userId, int skip, int take)
        {
            // todo implement
            return new List<Book>();
        }
    }
}
