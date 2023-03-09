namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class EmployerProfileUsersApiResponse
    {
        public string GovUkIdentifier { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public bool IsSuspended { get; set; }
    }
}