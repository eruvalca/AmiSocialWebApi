using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AmiSocialWebApi.Data;
using AmiSocialWebApi.Models;
using AmiSocialWebApi.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AmiSocialWebApi.Services
{
    public class UserService
    {
        private readonly UserManager<AmiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SymmetricKeyService _symmetricKeyService;

        public UserService(UserManager<AmiUser> userManager, IConfiguration configuration, SymmetricKeyService symmetricKeyService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _symmetricKeyService = symmetricKeyService;
        }

        public async Task<AuthResponse> RegisterUser(RegisterViewModel model)
        {
            var user = new AmiUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                FamilyNickname = model.FamilyNickname,
                DateOfBirth = model.DateOfBirth
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
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim("MiddleName", user.MiddleName ?? ""),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("FamilyNickname", user.FamilyNickname ?? ""),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToShortDateString())
            };

            var symmetricKey = _symmetricKeyService.GetSymmetricKey();

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "http://localhost:44366/",
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                IsSuccess = true,
                Token = tokenString
            };
        }

        public async Task<AmiUser> GetUser(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AuthResponse> UpdateUser(AmiUser user)
        {
            var amiUser = await GetUser(user.Email);

            amiUser.FirstName = user.FirstName;
            amiUser.MiddleName = user.MiddleName;
            amiUser.LastName = user.LastName;
            amiUser.FamilyNickname = user.FamilyNickname;
            amiUser.DateOfBirth = user.DateOfBirth;

            var result = await _userManager.UpdateAsync(amiUser);

            if (result.Succeeded)
            {
                return new AuthResponse { IsSuccess = true };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "There was an unexpected error updating the profile. Please try again." }
                };
            }
        }
    }
}