using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(new { status = "OK" });
        }
    }
}
