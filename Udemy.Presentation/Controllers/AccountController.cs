using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;

namespace Udemy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<ApplicationUser> userManager , ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.Fullname ,
                Email = model.Email ,
                UserName = model.Username
            };
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _userManager.FindByEmailAsync(user.Email) is not null)
            {
                return BadRequest("User Already Exists");
            }
            await _userManager.CreateAsync(user , model.Password);
            await _userManager.AddToRoleAsync(user , model.Role);
            return Ok(model);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login(string email , string password)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
                return NotFound();


            var Result = await _userManager.CheckPasswordAsync(User , password);
            if (!Result)
                return BadRequest("Wrong Email or Password");

            var model = new LoginDTO()
            {
                Email = email ,
                DisplayName = User.DisplayName ,
                Role = await _userManager.GetRolesAsync(User) ,
                Token = await _tokenService.CreateTokenAsync(User , _userManager)
            };
            return Ok(model);
        }
    }
}
