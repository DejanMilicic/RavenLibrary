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
        public async Task<Employee> Get(string id)
        {
            var cluster = await Cluster.ConnectAsync("couchbase://localhost", "admin", "password");

            // get a bucket reference
            var bucket = await cluster.BucketAsync("Library");

            // get a collection reference
            var collection = bucket.DefaultCollection();

            var res = await collection.GetAsync(id);

            Employee emp = res.ContentAs<Employee>();

            return emp;
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
            var upsertResult = await collection.UpsertAsync(id, new Employee { Name = "Ted", Age = 31 });
            var getResult = await collection.GetAsync(id);

            return id;
        }
    }

    public class Employee 
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
