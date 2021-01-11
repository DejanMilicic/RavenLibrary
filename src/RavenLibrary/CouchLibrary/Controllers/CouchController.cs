using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Couchbase;

namespace CouchLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CouchController : ControllerBase
    {
        [HttpGet("/get/")]
        public async Task<object> Get(string id)
        {
            var cluster = await Cluster.ConnectAsync("couchbase://localhost", "admin", "password");

            // get a bucket reference
            var bucket = await cluster.BucketAsync("Library");

            // get a collection reference
            var collection = bucket.DefaultCollection();

            var getResult = await collection.GetAsync(id);

            return getResult.ContentAs<string>();
        }

        [HttpPost]
        public async Task<string> Create()
        {
            var cluster = await Cluster.ConnectAsync("couchbase://localhost", "admin", "password");

            // get a bucket reference
            var bucket = await cluster.BucketAsync("Library");

            // get a collection reference
            var collection = bucket.DefaultCollection();

            string id = Guid.NewGuid().ToString();

            // Upsert Document
            var upsertResult = await collection.UpsertAsync(id, new { Name = "Ted", Age = 31 });
            var getResult = await collection.GetAsync(id);

            return getResult.ContentAs<string>();
        }
    }
}
