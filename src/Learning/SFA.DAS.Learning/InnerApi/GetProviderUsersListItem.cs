namespace SFA.DAS.Learning.InnerApi;

#pragma warning disable CS8618
public class GetProviderUsersListItem
{
    public string UserRef { get; set; }
    public string EmailAddress { get; set; }
    public string DisplayName { get; set; }
    public bool ReceiveNotifications { get; set; }
    public bool IsSuperUser { get; set; }
}
#pragma warning restore CS8618
