using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.InnerApi.Responses
{
    public class GetAccountUsersResponse : List<AccountUser>
    {
    }

    public class AccountUser
    {
        public string UserRef { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool CanReceiveNotifications { get; set; }
        public int Status { get; set; }
    }
}