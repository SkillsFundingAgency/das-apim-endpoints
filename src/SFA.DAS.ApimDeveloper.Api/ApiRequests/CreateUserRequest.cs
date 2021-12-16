namespace SFA.DAS.ApimDeveloper.Api.ApiRequests
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmationEmailLink { get ; set ; }
    }
}