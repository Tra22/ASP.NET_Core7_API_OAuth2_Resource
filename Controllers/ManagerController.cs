using Asp.Versioning;
using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Constants.Securities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_CORE7_API_OAUTH2_RESOURCE.Controllers {
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ManagerController : ControllerBase {
        [HttpGet]
        [Authorize(Scope.MANAGER_READ)]
        public IActionResult GetIndex() {
            return Ok($"GET: Accessed with policy {Scope.MANAGER_READ}");
        }
        [HttpPost]
        [Authorize(Scope.MANAGER_WRITE)]
        public IActionResult PostIndex() {
            return Ok($"POST: Accessed with policy {Scope.MANAGER_WRITE}");
        }
        [HttpPut]
        [Authorize(Scope.MANAGER_UPDATE)]
        public IActionResult PutIndex() {
            return Ok($"PUT: Accessed with policy {Scope.MANAGER_UPDATE}");
        }
        [HttpDelete]
        [Authorize(Scope.MANAGER_DELETE)]
        public IActionResult DeleteIndex() {
            return Ok($"DELETE: Accessed with policy {Scope.MANAGER_DELETE}");
        }
    }
}