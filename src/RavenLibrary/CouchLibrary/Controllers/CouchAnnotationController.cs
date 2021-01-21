using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly BucketContext _bc;

        private readonly IBucket _bucket;

        public CouchAnnotationController(BucketContext bc, IBucketProvider bucketProvider)
        {
            _bc = bc;
            _bucket = bucketProvider.GetBucket("Library");
        }

        [HttpGet("/annotation")]
        public string Get()
        {
            //_bucket.coll
            var ann = _bucket
                .Exists("Annotations/users/5101859-ebooks/10213/0000000002181037070-A");
            //var annotation = _bc.Query<Annotation>().FirstOrDefault(x => x.Id == id);
            return ann.ToString();
        }

        // Annotations/users/5101859-ebooks/10213/0000000002181037070-A
        // Annotations/users/5101859-ebooks/10213/0000000002181037070-A
        [HttpGet("/annotations/user/")]
        public List<Annotation> GetUserAnnotations(string userId)
        {
            return _bc.Query<Annotation>()
                .Where(x => x.Id.StartsWith($"Annotations/{userId}"))
                .ToList();
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
