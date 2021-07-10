using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AmiSocialWebApi.Models
{
    public class AmiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FamilyNickname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }

        public List<Post> Posts { get; set; }
    }
}