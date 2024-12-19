namespace SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings
{
    public class MemberNotificationEventFormatModel
    {
        public long Id { get; set; }
        public Guid MemberId { get; set; }
        public string EventFormat { get; set; } = string.Empty;
        public int Ordering { get; set; }
        public bool ReceiveNotifications { get; set; }
    }
}