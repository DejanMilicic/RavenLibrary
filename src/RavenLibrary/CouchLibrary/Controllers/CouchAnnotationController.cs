using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Couchbase.Linq;
using CouchLibrary.Models;
using RavenLibrary.Models;

namespace CouchLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CouchAnnotationController : ControllerBase
    {
        private readonly IBucket _bucket;



        public CouchAnnotationController(IBucket bucket)
        {
            _bucket = bucket;

        }

        [HttpGet("/smoketest")]
        public async Task<string> SmokeTest()
        {

            //_bucket.coll
            var ann = await _bucket.Collection("Annotations")
                .ExistsAsync("Annotations/users/5101859-ebooks/56717/0000000002180997833-A");
            //var annotation = _bc.Query<Annotation>().FirstOrDefault(x => x.Id == id);
            return ann.Exists.ToString();
        }

        [HttpGet("/annotation")]
        public async Task<Annotation> Get(string id)
        {
            var coll = _bucket.Collection("Annotations");
            return (await coll.GetAsync(id)).ContentAs<Annotation>();
        }

        // Annotations/users/5101859-ebooks/56717/0000000002180997833-A
        // Annotations/users/5101859-ebooks/56717/0000000002180997834-A
        [HttpGet("/annotations/user/")]
        public async Task<dynamic> GetUserAnnotations(string userId)
        {
            var res = await Startup.Cluster
                .QueryAsync<Annotation>("SELECT RAW a FROM Library._default.Annotations a where a.`user` = 'users/5101859'");

            return await res.Rows.ToListAsync();
        }

        //[HttpGet("/get/")]
        //public Employee Get(string id)
        //{
        //    return _bc.Query<Employee>().FirstOrDefault(x => x.Id == id);
        //}

        //[HttpPost]
        //public string Create()
        //{
        //    var employee = new Employee { Id = Guid.NewGuid().ToString(), Name = "Ted", Age = 33 };

        //    _bc.Save(employee);

        //    return employee.Id;
        //}
    }
}
