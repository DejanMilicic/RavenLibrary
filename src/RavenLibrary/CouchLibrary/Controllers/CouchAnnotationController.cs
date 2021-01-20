using System;
using System.Linq;
using System.Threading.Tasks;
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

        public CouchAnnotationController(BucketContext bc)
        {
            _bc = bc;
        }

        [HttpGet("/annotation")]
        public Annotation Get(string id)
        {
            return _bc.Query<Annotation>().FirstOrDefault(x => x.Id == id);
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
