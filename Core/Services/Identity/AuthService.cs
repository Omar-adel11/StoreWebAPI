using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServicesAbstractions.Identity;
using Shared;

namespace Services.Identity
{
    public class AuthService(UserManager<Appuser> _usermanager,IOptions<JWTOptions> _options) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto logindto)
        {
            var user = await _usermanager.FindByEmailAsync(logindto.Email);
            if (user is null) throw new UnauthorizedException();

            var flag = await _usermanager.CheckPasswordAsync(user, logindto.Password);
            if (!flag) throw new UnauthorizedException();
            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJWTTokenAsync(user)
            };
        }


        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new Appuser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await _usermanager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded) throw new RegisterationBadRequestException(result.Errors.Select(E=>E.Description));

            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJWTTokenAsync(user)
            };

        }

        
        private async Task<string> GenerateJWTTokenAsync(Appuser user)
        {
            var JWToptions = _options.Value;

            //CLAIMS
            var authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var Roles = await _usermanager.GetRolesAsync(user);
            foreach(var role in Roles)
            {
                authclaims.Add(new Claim(ClaimTypes.Role, role));
            }

            //KEY
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWToptions.Secretkey));

            //TOKEN

            var token = new JwtSecurityToken
            (
                issuer: JWToptions.Issuer,
                audience: JWToptions.Audience,
                claims: authclaims,
                expires: DateTime.UtcNow.AddDays(JWToptions.DurationInDays),
                signingCredentials: new SigningCredentials(secretkey,SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}
