using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmiSocialWebApi.Models.ViewModels;
using AmiSocialWebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmiSocialWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _userService.RegisterUser(model);

                if (registerResult.IsSuccess)
                {
                    //try to log user in
                    var loginModel = new LoginViewModel
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    var loginResult = await _userService.LoginUser(loginModel);

                    if (loginResult.IsSuccess)
                    {
                        //return token for login
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