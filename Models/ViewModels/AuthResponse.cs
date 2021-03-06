using System.Collections.Generic;

namespace AmiSocialWebApi.Models.ViewModels
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public List<string> Messages { get; set; }
    }
}