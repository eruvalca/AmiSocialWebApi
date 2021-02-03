using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AmiSocialWebApi.Services
{
    public class SymmetricKeyService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public SymmetricKeyService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public SymmetricSecurityKey GetSymmetricKey()
        {
            if (_env.EnvironmentName == Environments.Development)
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SymmetricKey"]));
            }
            else
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetConnectionString("SymmetricKey")));
            }
        }
    }
}