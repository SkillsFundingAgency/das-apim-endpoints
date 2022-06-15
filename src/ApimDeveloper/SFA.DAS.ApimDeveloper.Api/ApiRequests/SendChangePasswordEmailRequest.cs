namespace SFA.DAS.ApimDeveloper.Api.ApiRequests
{
    public class SendChangePasswordEmailRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ChangePasswordUrl { get; set; }
    }
}