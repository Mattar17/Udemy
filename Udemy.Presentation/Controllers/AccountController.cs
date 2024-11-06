using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager , ITokenService tokenService,IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
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

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user is null)
                return BadRequest();

            else
            {
                var email = new Email()
                {
                    To = Email ,
                    Subject = "Reset Password Link"
                };
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var resetLink = Url.Action("ResetPassword" , "Account" , new { Email = request.Email , Token = resetToken},Request.Scheme);
                email.Body = resetToken;
                await _emailService.SendEmailAsync(email);
                return Ok($"Reset Link was sent to {Email}");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.FindByEmailAsync (request.Email);

            if (user is null ) return BadRequest();

            var Result = await _userManager.ResetPasswordAsync(user , request.Token , request.NewPassword);

            if(!Result.Succeeded || request.ConfirmPassword != request.NewPassword)
                return BadRequest("Reset Password Failed!");


            return Ok("Password Reseted Successfully");
        }
    }
}
