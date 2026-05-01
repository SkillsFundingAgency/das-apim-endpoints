namespace SFA.DAS.SharedOuterApi.Types.Models.DfeSignIn
{
    public class OrganisationUsersResponse
    {
        public string Ukprn { get; set; } = "";
        public List<User> Users { get; set; } = new();
    }
}
