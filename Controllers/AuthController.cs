using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AmiSocialWebApi.Data;
using AmiSocialWebApi.Models;
using AmiSocialWebApi.Models.ViewModels;
using AmiSocialWebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmiSocialWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AppDbContext _context;

        public AuthController(UserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _userService.RegisterUser(registerModel);

                if (registerResult.IsSuccess)
                {
                    //try to log user in
                    var loginModel = new LoginViewModel
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password
                    };

                    var loginResult = await _userService.LoginUser(loginModel);

                    if (loginResult.IsSuccess)
                    {
                        //create member
                        var newMember = new Member
                        {
                            FirstName = registerModel.FirstName,
                            MiddleName = registerModel.MiddleName,
                            LastName = registerModel.LastName,
                            FamilyNickname = registerModel.FamilyNickname,
                            DateOfBirth = registerModel.DateOfBirth,
                        };

                        var user = await _userService.GetUser(loginModel.Email);

                        newMember.User = user;

                        await _context.Members.AddAsync(newMember);
                        await _context.SaveChangesAsync();

                        //return member and token for login
                        return Ok(loginResult);
                    }
                    else
                    {
                        //if login did not work just return register result
                        return Ok(registerResult);
                    }
                }
                else
                {
                    return BadRequest(registerResult);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                            .Where(m => m.Value.Errors.Count > 0)
                            .Select(m => m.Value.Errors.ToString())
                            .ToList()
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                            .Where(m => m.Value.Errors.Count > 0)
                            .Select(m => m.Value.Errors.ToString())
                            .ToList()
                });
            }
        }
    }
}