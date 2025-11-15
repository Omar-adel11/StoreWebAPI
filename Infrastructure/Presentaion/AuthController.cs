using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using ServicesAbstractions.Identity;
using Shared.DTOs.Identity;

namespace Presentaion
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _serviceManager.authService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _serviceManager.authService.RegisterAsync(registerDto);
            return Ok(result);
        }

        [HttpGet("EmailExists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var exists = await _serviceManager.authService.CheckEmailExistsAsync(email);
            return Ok(exists);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var user = await _serviceManager.authService.GetCurrentUserAsync(email.Value);
            return Ok(user);
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<IActionResult> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var address = await _serviceManager.authService.GetUserAddressAsync(email);
            return Ok(address);
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAddress(AddressDto address)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var updatedAddress = await _serviceManager.authService.GetUserAddressAsync(address, email);
            return Ok(updatedAddress);
        }
    }
}
