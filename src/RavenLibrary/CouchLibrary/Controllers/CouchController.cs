using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Couchbase;

namespace CouchLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CouchController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "cOuch!";
        }

        [HttpPost]
        public async Task<string> Create()
        {
            var cluster = await Cluster.ConnectAsync("couchbase://localhost", "user", "password");

            // get a bucket reference
            var bucket = await cluster.BucketAsync("Library");

            // get a collection reference
            var collection = bucket.DefaultCollection();

            // Upsert Document
            var upsertResult = await collection.UpsertAsync("my-document-key", new { Name = "Ted", Age = 31 });
            var getResult = await collection.GetAsync("my-document-key");

            return getResult.ContentAs<dynamic>();
        }
    }
}
