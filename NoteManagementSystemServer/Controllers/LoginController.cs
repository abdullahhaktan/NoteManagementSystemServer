using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteManagemenSystemServer.Data.DTOs.UserDtos;
using NoteManagemenSystemServer.Data.Entities;
using NoteManagementSystemServer.Services.TokenServices;

namespace NoteManagemenSystemServer.Controllers
{
    // This controller is open to everyone, no JWT token required.
    // Since we defined a global AuthorizeFilter in Program.cs,
    // we need [AllowAnonymous] to keep this endpoint accessible.
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<LoginDto> _validator;

        public LoginController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IValidator<LoginDto> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Validate incoming data before hitting the database
            var result = await _validator.ValidateAsync(loginDto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null)
                return Unauthorized("Kullanıcı adı veya şifre hatalı!");

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid)
                return Unauthorized("Kullanıcı adı veya şifre hatalı!");

            // Credentials are valid — generate JWT token
            var token = await _tokenService.CreateTokenAsync(user);

            // Return UserDto instead of entity to avoid exposing sensitive fields
            return Ok(new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token,
            });
        }
    }
}