using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Models.DfeSignIn
{
    public class OrganisationUsersResponse
    {
        public string Ukprn { get; set; } = "";
        public List<User> Users { get; set; } = new();
    }
}
