namespace SFA.DAS.Apprenticeships.InnerApi
{
    public class GetProviderUsersListItem
    {
        public string UserRef { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public bool ReceiveNotifications { get; set; }
        public bool IsSuperUser { get; set; }
    }
}
