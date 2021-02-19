using System.Threading.Tasks;
using AmiSocialWebApi.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AmiSocialWebApi.Services
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterUser(RegisterViewModel model);

        Task<AuthResponse> LoginUser(LoginViewModel model);
    }
}