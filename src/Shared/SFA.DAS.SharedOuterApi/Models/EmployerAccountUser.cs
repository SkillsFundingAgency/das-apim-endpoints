namespace SFA.DAS.SharedOuterApi.Models
{
    public class EmployerAccountUser
    {
        public string DasAccountName {get;set;}
        public string EncodedAccountId {get;set;}
        public string Role {get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
    }
}