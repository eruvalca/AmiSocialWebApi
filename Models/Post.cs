using System;

namespace AmiSocialWebApi.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string AmiUserId { get; set; }
    }
}