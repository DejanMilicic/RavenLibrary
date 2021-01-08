using Microsoft.AspNetCore.Mvc;

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
    }
}
