using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_CORE7_API_OAUTH2_RESOURCE.Controllers {
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProtectController : ControllerBase {
        [HttpGet]
        [Route("protect")]
        [Authorize]
        public IActionResult GetProtectIndex(){return Ok("test");}
    }
}