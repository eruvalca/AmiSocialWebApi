using System;

namespace AmiSocialWebApi.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FamilyNickname { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }
    }
}