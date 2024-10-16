using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
    {
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser { 
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return NoContent();
            }

            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

            return Ok(new { 
                user.Email,
                user.FirstName,
                user.LastName,
                Address = user.Address.ToDto()
            });

        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult GetAuthState()
        {
           return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

            if (user.Address == null)
            {
                user.Address = addressDto.ToEntity();
            }
            else
            {
                user.Address.UpdateFromDto(addressDto);
            }

            var result = await signInManager.UserManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Problem updating with user address");

            return Ok(user.Address.ToDto()) ;
        }
    }
}
