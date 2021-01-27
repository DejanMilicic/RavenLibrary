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
using Couchbase.Query;

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
            var ann = await _bucket.Collection("Annotations")
                .ExistsAsync("Annotations/users/5101859-ebooks/56717/0000000002180997833-A");
            return ann.Exists.ToString();
        }

        [HttpGet("/annotation")]
        public async Task<Annotation> Get(string id)
        {
            var coll = _bucket.Collection("Annotations");
            return (await coll.GetAsync(id)).ContentAs<Annotation>();
        }

        [HttpGet("/annotations/user")]
        public async Task<List<Annotation>> GetUserAnnotations(string userId)
        {
            string query = "SELECT RAW a FROM Library._default.Annotations a where a.`user` = ?";

            var res = await Startup.Cluster.QueryAsync<Annotation>(query, options => options.Parameter(userId).AdHoc(false));

            return await res.Rows.ToListAsync();
        }

        [HttpGet("/annotations/user/{skip}/{take}")]
        public async Task<List<Annotation>> GetUserAnnotationsRange(string userId, int skip, int take)
        {
            string query = "SELECT RAW a FROM Library._default.Annotations a where a.`user` = ? offset ? limit ?";

            var res = await Startup.Cluster.QueryAsync<Annotation>(query, options=> options.AdHoc(false).Parameter(userId).Parameter(skip).Parameter(take));

            return await res.Rows.ToListAsync();
        }

        [HttpGet("/annotations/userbook")]
        public async Task<List<Annotation>> GetUserBookAnnotations(string userId)
        {
            string query = "SELECT RAW a FROM Library._default.Annotations a where META().id LIKE ?";

            var res = await Startup.Cluster.QueryAsync<Annotation>(query, options => options.Parameter($"Annotations/{userId}-%").AdHoc(false));

            return await res.Rows.ToListAsync();
        }

        [HttpGet("/annotations/userbook/{skip}/{take}")]
        public async Task<List<Annotation>> GetUserBookAnnotations(string userId, int skip, int take)
        {
            string query = "SELECT RAW a FROM Library._default.Annotations a where META().id LIKE ? offset ? limit ?";

            var res = await Startup.Cluster.QueryAsync<Annotation>(query, options => options.Parameter($"Annotations/{userId}-%").Parameter(skip).Parameter(take).AdHoc(false));

            return await res.Rows.ToListAsync();
        }

          [HttpGet("/userbooks")]
        public async Task<List<Annotation>> GetUserBooks(string userId)
        {
            string query = "SELECT RAW a FROM Library._default.UserBooks a where META().id LIKE ?";

            var res = await Startup.Cluster.QueryAsync<Annotation>(query, options => options.Parameter($"UsersBooks/{userId}-%").AdHoc(false));

            return await res.Rows.ToListAsync();
        }
    }
}
