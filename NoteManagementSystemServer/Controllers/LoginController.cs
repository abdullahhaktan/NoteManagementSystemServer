using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteManagemenSystemServer.Data.DTOs.UserDtos;
using NoteManagemenSystemServer.Data.Entities;
using NoteManagementSystemServer.Services.TokenServices;

namespace NoteManagemenSystemServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginController(UserManager<AppUser> userManager, 
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı!");
            }

            // Şifreyi kontrol et
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı!");
            }

            // Token üret
            var token = await _tokenService.CreateTokenAsync(user);

            // Response döndür
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
