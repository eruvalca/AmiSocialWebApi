using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AmiSocialWebApi.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AmiSocialWebApi.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SymmetricKeyService _symmetricKeyService;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, SymmetricKeyService symmetricKeyService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _symmetricKeyService = symmetricKeyService;
        }

        public async Task<AuthResponse> RegisterUser(RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = true,
                    Messages = new List<string> { "User registered successfully" }
                };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        public async Task<AuthResponse> LoginUser(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "User does not exist" }
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "Invalid Password" }
                };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var symmetricKey = _symmetricKeyService.GetSymmetricKey();

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "http://localhost:44366/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                IsSuccess = true,
                Token = tokenString
            };
        }

        public async Task<IdentityUser> GetUser(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}