using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("Unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("BadRequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("NotFound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("InternalError")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception");
        }

        [HttpPost("ValidationError")]
        public IActionResult GetValidationError(CreateProductDto product)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name).Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return Ok("test");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-secret")]
        public IActionResult GetAdminSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name).Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isAdmin = User.IsInRole("Admin");
            var roles = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                name,
                id,
                isAdmin,
                roles
            });
        }
    }
}
