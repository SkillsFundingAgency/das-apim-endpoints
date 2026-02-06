using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Models.DfeSignIn
{
    public class User
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        //public int UserStatus { get; set; }
        //public List<string> Roles { get; set; } = new();
    }
}